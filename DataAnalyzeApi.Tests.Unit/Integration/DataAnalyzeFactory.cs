using DataAnalyzeApi.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DataAnalyzeApi.Tests.Integration;

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

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveProductionServices(services);
            AddTestServices(services);
        });
    }

    private void RemoveProductionServices(IServiceCollection services)
    {
        var descriptorsToRemove = new[]
        {
            typeof(DbContextOptions<DataAnalyzeDbContext>),
            typeof(IConnectionMultiplexer)
        };

        foreach (var serviceType in descriptorsToRemove)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == serviceType);

            if (descriptor != null)
                services.Remove(descriptor);
        }
    }

    private void AddTestServices(IServiceCollection services)
    {
        // Add database context using test container connection string
        services.AddDbContext<DataAnalyzeDbContext>(options =>
            options.UseNpgsql(postgresConnectionString));

        // Add Redis using test container connection string
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConnectionString));
    }
}
