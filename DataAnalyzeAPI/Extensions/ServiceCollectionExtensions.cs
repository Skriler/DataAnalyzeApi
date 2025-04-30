using DataAnalyzeApi.Services.Analyse.Metrics;
using DataAnalyzeApi.DAL;
using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.DAL.Seeders;
using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Middlewares;
using DataAnalyzeApi.Models.Config;
using DataAnalyzeApi.Models.Config.Identity;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Services.Analyse.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Comparers;
using DataAnalyzeApi.Services.Analyse.Core;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Services.Analyse.Helpers;
using DataAnalyzeApi.Services.Analyse.Metrics.Categorical;
using DataAnalyzeApi.Services.Analyse.Metrics.Numeric;
using DataAnalyzeApi.Services.Auth;
using DataAnalyzeApi.Services.Cache;
using DataAnalyzeApi.Services.DataPreparation;
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
        services.AddSwaggerGen();
        services
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

        // Factories
        services.AddScoped<MetricFactory>();

        // Core services
        services.AddScoped<IDistanceCalculator, DistanceCalculator>();

        return services;
    }

    private static IServiceCollection AddClusteringServices(this IServiceCollection services)
    {
        // Clusterers
        services.AddScoped<KMeansClusterer>();
        services.AddScoped<DBSCANClusterer>();
        services.AddScoped<AgglomerativeClusterer>();

        // Factories
        services.AddScoped<ClustererFactory>();

        // Core services
        services.AddScoped<ClusteringService>();

        return services;
    }
}
