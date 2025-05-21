using Castle.Core.Configuration;
using DataAnalyzeApi.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using StackExchange.Redis;

namespace DataAnalyzeApi.Tests.Integration;

public class DataAnalyzeFactory : WebApplicationFactory<Program>
{
    private string postgresConnectionString;
    private string redisConnectionString;

    public DataAnalyzeFactory()
    {
        postgresConnectionString = "Host=localhost;Database=dataanalyze_test;Username=postgres;Password=postgres";
        redisConnectionString = "localhost:6379";
    }

    public void UpdateConnectionStrings(string postgresConnectionString, string redisConnectionString)
    {
        this.postgresConnectionString = postgresConnectionString;
        this.redisConnectionString = redisConnectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configBuilder =>
        {
            // Build base configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Testing.json", optional: true)
                .Build();

            // Add memory configuration with test settings
            var testConfig = new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = postgresConnectionString,
                ["Redis:ConnectionString"] = redisConnectionString,
                ["Redis:InstanceName"] = "DataAnalyzeTest:",
                ["Redis:DefaultCacheDurationMinutes"] = "1",
                ["JwtConfig:Secret"] = "very-long-secret-key-for-testing-purposes-only-do-not-use-in-production",
                ["JwtConfig:Issuer"] = "DataAnalyzeApiTest",
                ["JwtConfig:Audience"] = "DataAnalyzeApiClientTest",
                ["JwtConfig:ExpirationMinutes"] = "60",
                ["Identity:AdminUser:Password"] = "YourAdminPasswordForTesting" // Test password
            };

            configBuilder.AddInMemoryCollection(testConfig);
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DataAnalyzeDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<DataAnalyzeDbContext>(options =>
            {
                
            });

            var redisDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IConnectionMultiplexer));

            if (redisDescriptor != null)
            {
                services.Remove(redisDescriptor);
            }

            // Add database context using test container connection string
            services.AddDbContext<DataAnalyzeDbContext>(options =>
            {
                options.UseNpgsql(postgresConnectionString);
            });

            // Add Redis using test container connection string
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });

            // Ensure database is created and migrated for tests
            services.BuildServiceProvider().CreateScope().ServiceProvider
                .GetRequiredService<DataAnalyzeDbContext>().Database.Migrate();

            // Seed test data as needed
            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<DataAnalyzeDbContext>();

                // Ensure database is created and seeded
                db.Database.EnsureCreated();
                SeedTestDatabase(db);
            }
        });
    }

    private void SeedTestDatabase(DataAnalyzeDbContext dbContext)
    {
        // Add any test data needed for all tests
        // For example, add a test user with the Admin role if needed

        // Ensure we don't add duplicate data
        if (!dbContext.Users.Any(u => u.Email == "admin@gmail.com"))
        {
            // Add test admin user
            // Note: In a real implementation, you would hash the password
            // This is simplified for demonstration purposes
            // You may want to use your actual user entity and authentication service
        }

        dbContext.SaveChanges();
    }
}
