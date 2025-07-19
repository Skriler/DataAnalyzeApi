using System.Text;
using DataAnalyzeApi.DAL;
using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.DAL.Repositories.Analysis;
using DataAnalyzeApi.DAL.Seeders;
using DataAnalyzeApi.Mappers.Analysis.Domain;
using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Mappers.Analysis.Profiles;
using DataAnalyzeApi.Mappers.Entities;
using DataAnalyzeApi.Mappers.Entities.Profiles;
using DataAnalyzeApi.Mappers.Profiles.Entities;
using DataAnalyzeApi.Middlewares;
using DataAnalyzeApi.Models.Config;
using DataAnalyzeApi.Models.Config.Identity;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analysis.Clustering.Helpers;
using DataAnalyzeApi.Services.Analysis.Comparers;
using DataAnalyzeApi.Services.Analysis.Core;
using DataAnalyzeApi.Services.Analysis.DimensionalityReducers;
using DataAnalyzeApi.Services.Analysis.DimensionalityReducers.PcaHelpers;
using DataAnalyzeApi.Services.Analysis.DistanceCalculators;
using DataAnalyzeApi.Services.Analysis.Factories.Clusterer;
using DataAnalyzeApi.Services.Analysis.Factories.Metric;
using DataAnalyzeApi.Services.Analysis.Metrics.Categorical;
using DataAnalyzeApi.Services.Analysis.Metrics.Numeric;
using DataAnalyzeApi.Services.Analysis.Results;
using DataAnalyzeApi.Services.Auth;
using DataAnalyzeApi.Services.Cache;
using DataAnalyzeApi.Services.Normalizers;
using DataAnalyzeApi.Services.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DataAnalyzeApi.Extensions.Core;

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
            .AddScoped<SimilarityAnalysisResultRepository>()
            .AddScoped<ClusteringAnalysisResultRepository>()
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
            .AddDimensionalityReducers()
            .AddClusteringServices()
            .AddFactories()
            .AddFilters()
            .AddValidators();
    }

    private static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICacheService, DistributedCacheService>()
            .AddScoped<AnalysisCacheService<SimilarityAnalysisResultDto>>()
            .AddScoped<AnalysisCacheService<ClusteringAnalysisResultDto>>();
    }

    private static IServiceCollection AddMappers(this IServiceCollection services)
    {  
        return services
            .AddAutoMapper(typeof(DatasetReadProfile))
            .AddAutoMapper(typeof(DatasetCreateProfile))
            .AddAutoMapper(typeof(SimilarityAnalysisResultProfile))
            .AddAutoMapper(typeof(ClusteringAnalysisResultProfile))
            .AddScoped<DatasetSettingsMapper>()
            .AddScoped<SimilarityDomainAnalysisMapper>()
            .AddScoped<ClusteringDomainAnalysisMapper>()
            .AddScoped<SimilarityEntityAnalysisMapper>()
            .AddScoped<ClusteringEntityAnalysisMapper>();
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
        services.AddScoped<SimilarityAnalysisResultService>();
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

    private static IServiceCollection AddDimensionalityReducers(this IServiceCollection services)
    {
        // Helpers
        services.AddScoped<MatrixProcessor>();
        services.AddScoped<EigenSolver>();

        // Core services
        services.AddScoped<IDimensionalityReducer, PcaReducer>();

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
        services.AddScoped<ClusteringAnalysisResultService>();
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
            .AddScoped<DataAnalysisExceptionFilter>();
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<DatasetValidator>();
    }
}
