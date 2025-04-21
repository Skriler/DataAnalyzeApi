using DataAnalyzeAPI.DAL;
using DataAnalyzeAPI.DAL.Repositories;
using DataAnalyzeAPI.DAL.Seeders;
using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.Config;
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

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataAnalyzeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.Configure<RedisConfig>(configuration.GetSection("Redis"));

        services.AddStackExchangeRedisCache(options =>
        {
            var redisConfig = configuration.GetSection("Redis").Get<RedisConfig>();
            options.Configuration = redisConfig?.ConnectionString;
            options.InstanceName = redisConfig?.InstanceName;
        });

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DataAnalyzeDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

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
                    Encoding.UTF8.GetBytes(configuration["JwtConfig:Secret"]))
            };
        });

        services.AddControllers();
        services.AddSwaggerGen();

        services.AddDal()
            .AddCacheServices()
            .AddAuthServices()
            .AddMappers()
            .AddHelpers()
            .AddDataProcessingServices()
            .AddDistanceServices()
            .AddClusteringServices();

        return services;
    }

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Data Analyze API v1");
                opt.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        return app;
    }

    private static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddScoped<DatasetRepository>();
        services.AddScoped<IdentitySeeder>();

        return services;
    }

    private static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, DistributedCacheService>();
        services.AddScoped<ClusteringCacheService>();
        services.AddScoped<SimilarityCacheService>();

        return services;
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<JwtTokenService, JwtTokenService>();

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
        services.AddTransient<EuclideanDistanceMetric>();
        services.AddTransient<ManhattanDistanceMetric>();
        services.AddTransient<CosineDistanceMetric>();
        services.AddTransient<HammingDistanceMetric>();
        services.AddTransient<JaccardDistanceMetric>();

        services.AddTransient(provider =>
        {
            return new Dictionary<NumericDistanceMetricType, INumericDistanceMetric>
            {
                { NumericDistanceMetricType.Euclidean, provider.GetRequiredService<EuclideanDistanceMetric>() },
                { NumericDistanceMetricType.Manhattan, provider.GetRequiredService<ManhattanDistanceMetric>() },
                { NumericDistanceMetricType.Cosine, provider.GetRequiredService<CosineDistanceMetric>() }
            };
        });

        services.AddTransient(provider =>
        {
            return new Dictionary<CategoricalDistanceMetricType, ICategoricalDistanceMetric>
            {
                { CategoricalDistanceMetricType.Hamming, provider.GetRequiredService<HammingDistanceMetric>() },
                { CategoricalDistanceMetricType.Jaccard, provider.GetRequiredService<JaccardDistanceMetric>() }
            };
        });

        services.AddScoped<IDistanceCalculator, DistanceCalculator>();

        return services;
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
