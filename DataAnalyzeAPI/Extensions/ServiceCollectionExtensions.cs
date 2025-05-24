using DataAnalyzeApi.DAL;
using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.DAL.Seeders;
using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Middlewares;
using DataAnalyzeApi.Models.Config;
using DataAnalyzeApi.Models.Config.Identity;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Comparers;
using DataAnalyzeApi.Services.Analyse.Core;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Services.Analyse.Metrics.Categorical;
using DataAnalyzeApi.Services.Analyse.Metrics.Numeric;
using DataAnalyzeApi.Services.Auth;
using DataAnalyzeApi.Services.Cache;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.Factories.Clusterer;
using DataAnalyzeApi.Services.Analyse.Factories.Metric;
using DataAnalyzeApi.Services.Normalizers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DataAnalyzeApi.Extensions;

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
        services
            .AddDatabase(configuration)
            .AddCaching(configuration)
            .AddRepositories();

        return services;
    }

    /// <summary>
    /// Configures and registers the DbContext for PostgreSQL based on configuration.
    /// </summary>
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("Postgres").Get<PostgresConfig>()!;
        var connectionString =
            $"Host={config.Host};" +
            $"Port={config.Port};" +
            $"Database={config.Name};" +
            $"Username={config.Username};" +
            $"Password={config.Password}";

        services.AddDbContext<DataAnalyzeDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    /// <summary>
    /// Configures and registers Redis caching based on configuration.
    /// </summary>
    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisConfig>(configuration.GetSection("Redis"));

        var config = configuration.GetSection("Redis").Get<RedisConfig>()!;
        var connectionString = $"{config.Host}:{config.Port}";

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = config.InstanceName;
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<DatasetRepository>()
            .AddScoped<IdentitySeeder>();
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

        // Authorization
        services.AddAuthorization(options =>
        {
            var identityConfig = configuration.GetSection("Identity").Get<IdentityConfig>();
            var adminRole = identityConfig!.AdminRole;
            var defaultRole = identityConfig!.DefaultRole;

            options.AddPolicy("OnlyAdmin", policy => policy.RequireRole(adminRole));
            options.AddPolicy("UserOrAdmin", policy => policy.RequireRole(defaultRole, adminRole));
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
        services
            .AddSwaggerGen()
            .AddControllers()
            .AddDataAnalyzeExceptionFilters();

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
            .AddClusteringServices()
            .AddFactories()
            .AddFilters();
    }

    private static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICacheService, DistributedCacheService>()
            .AddScoped<ClusteringCacheService>()
            .AddScoped<SimilarityCacheService>();
    }

    private static IServiceCollection AddMappers(this IServiceCollection services)
    {
        return services
            .AddAutoMapper(typeof(DatasetProfile))
            .AddScoped<DatasetSettingsMapper>()
            .AddScoped<AnalysisMapper>();
    }

    private static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        return services
            .AddScoped<ClusterNameGenerator>();
    }

    private static IServiceCollection AddDataProcessingServices(this IServiceCollection services)
    {
        // Data preprocessing services
        services.AddScoped<DatasetNormalizer>();
        services.AddScoped<DatasetService>();

        // Similarity comparison services
        services.AddScoped<SimilarityComparer>();
        services.AddTransient<ICompare, NormalizedValueComparer>();

        // Core services
        services.AddScoped<SimilarityService>();

        return services;
    }

    private static IServiceCollection AddDistanceServices(this IServiceCollection services)
    {
        // Metrics
        services.AddTransient<EuclideanDistanceMetric>();
        services.AddTransient<ManhattanDistanceMetric>();
        services.AddTransient<CosineDistanceMetric>();
        services.AddTransient<HammingDistanceMetric>();
        services.AddTransient<JaccardDistanceMetric>();

        // Core services
        services.AddScoped<IDistanceCalculator, DistanceCalculator>();

        return services;
    }

    private static IServiceCollection AddClusteringServices(this IServiceCollection services)
    {
        // Helpers
        services.AddTransient<CentroidCalculator>();

        // Clusterers
        services.AddScoped<KMeansClusterer>();
        services.AddScoped<DBSCANClusterer>();
        services.AddScoped<AgglomerativeClusterer>();

        // Core services
        services.AddScoped<ClusteringService>();

        return services;
    }

    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        return services
            .AddScoped<IMetricFactory, MetricFactory>()
            .AddScoped<IClustererFactory, ClustererFactory>();
    }

    private static IServiceCollection AddFilters(this IServiceCollection services)
    {
        return services
            .AddScoped<DataAnalyzeExceptionFilter>();
    }
}
