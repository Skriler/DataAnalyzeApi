using DataAnalyzeApi.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DataAnalyzeApi.Integration.Common;

/// <summary>
/// Custom WebApplicationFactory for integration tests
/// that configures test database and Redis connections.
/// </summary>
public class DataAnalyzeFactory : WebApplicationFactory<Program>
{
    private string postgresConnectionString = string.Empty;
    private string redisConnectionString = string.Empty;

    public void UpdateConnectionStrings(
        string postgresConnectionString,
        string redisConnectionString)
    {
        this.postgresConnectionString = postgresConnectionString;
        this.redisConnectionString = redisConnectionString;
    }

    /// <summary>
    /// Configures the web host for the test environment.
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveProductionServices(services);
            AddTestServices(services);
        });
    }

    /// <summary>
    /// Removes production database and Redis services from DI container.
    /// </summary>
    private static void RemoveProductionServices(IServiceCollection services)
    {
        var descriptorsToRemove = new[]
        {
            typeof(DbContextOptions<DataAnalyzeDbContext>),
            typeof(IDistributedCache)
        };

        foreach (var serviceType in descriptorsToRemove)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == serviceType);

            if (descriptor != null)
                services.Remove(descriptor);
        }
    }

    /// <summary>
    /// Adds test database context and Redis connection using test container strings.
    /// </summary>
    private void AddTestServices(IServiceCollection services)
    {
        services.AddDbContext<DataAnalyzeDbContext>(options =>
            options.UseNpgsql(postgresConnectionString));

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "TestInstance";
        });
    }
}
