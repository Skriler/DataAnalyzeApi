using DataAnalyzeApi.DAL;
using DataAnalyzeApi.DAL.Seeders;
using DataAnalyzeApi.Models.Config;
using DataAnalyzeApi.Models.Config.Identity;
using DataAnalyzeApi.Models.DTOs.Auth;
using DataAnalyzeApi.Models.Entities;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace DataAnalyzeApi.Tests.Integration;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly DataAnalyzeFactory factory;
    protected readonly HttpClient client;

    protected IConfigurationRoot configuration;

    protected PostgreSqlContainer postgresContainer;
    protected RedisContainer redisContainer;

    protected string authToken = string.Empty;

    protected IntegrationTestBase()
    {
        factory = new DataAnalyzeFactory();
        client = factory.CreateClient();

        configuration = GetConfiguration();
        InitializeContainers();
    }

    public async Task InitializeAsync()
    {
        // Start containers
        await postgresContainer.StartAsync();
        await redisContainer.StartAsync();

        // Update connection strings in the factory
        factory.UpdateConnectionStrings(
            postgresContainer.GetConnectionString(),
            redisContainer.GetConnectionString());

        // Seed the database
        await SeedDatabaseAsync();

        // Get auth token
        await AuthenticateAsync();
    }

    public async Task DisposeAsync()
    {
        // Stop containers
        await postgresContainer.StopAsync();
        await redisContainer.StopAsync();
    }

    private IConfigurationRoot GetConfiguration()
    {
        DotNetEnv.Env.Load(".env.test");

        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.test.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

    private void InitializeContainers()
    {
        var postgresConfig = configuration
            .GetSection("Postgres")
            .Get<PostgresConfig>()!;

        // Initialize PostgreSQL container
        postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres")
            .WithPortBinding(5432, true)
            .WithDatabase(postgresConfig.Name)
            .WithUsername(postgresConfig.Username)
            .WithPassword(postgresConfig.Password)
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

        // Initialize Redis container
        redisContainer = new RedisBuilder()
            .WithImage("redis")
            .WithPortBinding(6379, true)
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
            .Build();
    }

    private async Task SeedDatabaseAsync()
    {
        using var scope = factory.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        // Get required services
        var dbContext = serviceProvider.GetRequiredService<DataAnalyzeDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = serviceProvider.GetRequiredService<ILogger<IdentitySeeder>>();
        var identityOptions = serviceProvider.GetRequiredService<IOptions<IdentityConfig>>();

        // Create identity seeder
        var identitySeeder = new IdentitySeeder(
            userManager,
            roleManager,
            identityOptions,
            logger
        );

        // Apply migrations and seed database
        await dbContext.Database.MigrateAsync();
        await identitySeeder.SeedAsync();
    }

    protected async Task AuthenticateAsync()
    {
        var adminConfig = configuration
            .GetSection("Identity")
            .GetSection("AdminUser")
            .Get<AdminUserConfig>()!;

        var loginDto = new LoginDto(adminConfig.Username, adminConfig.Password);
        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to authenticate: {await response.Content.ReadAsStringAsync()}");
        }

        var authResult = await response.Content.ReadFromJsonAsync<AuthResult>();
        authToken = authResult.Token;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
    }
}
