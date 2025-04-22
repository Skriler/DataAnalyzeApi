using DataAnalyzeAPI.DAL;
using DataAnalyzeAPI.DAL.Repositories;
using DataAnalyzeAPI.DAL.Seeders;
using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.Config;
using DataAnalyzeAPI.Models.Config.Identity;
using DataAnalyzeAPI.Models.Entities;
using DataAnalyzeAPI.Models.Enums;
using DataAnalyzeAPI.Services.Analyse.Clusterers;
using DataAnalyzeAPI.Services.Analyse.Comparers;
using DataAnalyzeAPI.Services.Analyse.DistanceCalculators;
using DataAnalyzeAPI.Services.Analyse.Helpers;
using DataAnalyzeAPI.Services.Analyse.Metrics.Categorical;
using DataAnalyzeAPI.Services.Analyse.Metrics.Numeric;
using DataAnalyzeAPI.Services.Auth;
using DataAnalyzeAPI.Services.Cache;
using DataAnalyzeAPI.Services.Normalizers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DataAnalyzeAPI.Extensions;

/// <summary>
/// Contains extension methods for registering application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures all services for the application.
    /// </summary>
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInfrastructure(configuration)
            .AddSecurityServices(configuration)
            .AddApiServices()
            .AddApplicationServices();

        return services;
    }

    /// <summary>
    /// Configures infrastructure-related services, including database and caching.
    /// </summary>
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<DataAnalyzeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        // Caching
        services.Configure<RedisConfig>(configuration.GetSection("Redis"));
        services.AddStackExchangeRedisCache(options =>
        {
            var redisConfig = configuration.GetSection("Redis").Get<RedisConfig>();
            options.Configuration = redisConfig?.ConnectionString;
            options.InstanceName = redisConfig?.InstanceName;
        });

        // Repositories and seeders
        services.AddScoped<DatasetRepository>();
        services.AddScoped<IdentitySeeder>();

        return services;
    }

    /// <summary>
    /// Configures security-related services, including authentication and identity.
    /// </summary>
    private static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
        services.Configure<IdentityConfig>(configuration.GetSection("Identity"));

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DataAnalyzeDbContext>()
            .AddDefaultTokenProviders();

        // Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtConfig:Issuer"],
                ValidAudience = configuration["JwtConfig:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JwtConfig:Secret"]!))
            };
        });

        // Auth services
        services.AddScoped<JwtTokenService>();
        services.AddScoped<AuthService>();

        return services;
    }

    /// <summary>
    /// Configures API-related services, including controllers and Swagger documentation.
    /// </summary>
    private static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();

        return services;
    }

    /// <summary>
    /// Configures application-specific services, including caching, mappers, data processing, and clustering.
    /// </summary>
    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddCacheServices()
            .AddMappers()
            .AddHelpers()
            .AddDataProcessingServices()
            .AddDistanceServices()
            .AddClusteringServices();
    }

    private static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, DistributedCacheService>();
        services.AddScoped<ClusteringCacheService>();
        services.AddScoped<SimilarityCacheService>();

        return services;
    }

    private static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DatasetProfile));
        services.AddScoped<DatasetSettingsMapper>();
        services.AddScoped<AnalysisMapper>();

        return services;
    }

    private static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddScoped<ClusterNameGenerator>();

        return services;
    }

    private static IServiceCollection AddDataProcessingServices(this IServiceCollection services)
    {
        services.AddScoped<DatasetNormalizer>();
        services.AddScoped<SimilarityComparer>();
        services.AddTransient<ICompare, NormalizedValueComparer>();

        return services;
    }

    private static IServiceCollection AddDistanceServices(this IServiceCollection services)
    {
        // Register metrics
        services.AddTransient<EuclideanDistanceMetric>();
        services.AddTransient<ManhattanDistanceMetric>();
        services.AddTransient<CosineDistanceMetric>();
        services.AddTransient<HammingDistanceMetric>();
        services.AddTransient<JaccardDistanceMetric>();

        // Register metric dictionaries
        services.AddTransient(CreateNumericDistanceMetricDictionary);
        services.AddTransient(CreateCategoricalDistanceMetricDictionary);

        services.AddScoped<IDistanceCalculator, DistanceCalculator>();

        return services;
    }

    private static Dictionary<NumericDistanceMetricType, INumericDistanceMetric> CreateNumericDistanceMetricDictionary(IServiceProvider provider)
    {
        return new Dictionary<NumericDistanceMetricType, INumericDistanceMetric>
        {
            { NumericDistanceMetricType.Euclidean, provider.GetRequiredService<EuclideanDistanceMetric>() },
            { NumericDistanceMetricType.Manhattan, provider.GetRequiredService<ManhattanDistanceMetric>() },
            { NumericDistanceMetricType.Cosine, provider.GetRequiredService<CosineDistanceMetric>() }
        };
    }

    private static Dictionary<CategoricalDistanceMetricType, ICategoricalDistanceMetric> CreateCategoricalDistanceMetricDictionary(IServiceProvider provider)
    {
        return new Dictionary<CategoricalDistanceMetricType, ICategoricalDistanceMetric>
        {
            { CategoricalDistanceMetricType.Hamming, provider.GetRequiredService<HammingDistanceMetric>() },
            { CategoricalDistanceMetricType.Jaccard, provider.GetRequiredService<JaccardDistanceMetric>() }
        };
    }

    private static IServiceCollection AddClusteringServices(this IServiceCollection services)
    {
        services.AddScoped<ClustererFactory>();
        services.AddScoped<KMeansClusterer>();
        services.AddScoped<DBSCANClusterer>();
        services.AddScoped<AgglomerativeClusterer>();

        return services;
    }
}
