using DataAnalyzeApi.DAL;
using DataAnalyzeApi.DAL.Seeders;
using DataAnalyzeApi.Integration.Common;
using DataAnalyzeApi.Models.Config;
using DataAnalyzeApi.Models.Config.Identity;
using DataAnalyzeApi.Models.DTOs.Auth;
using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.Entities;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace DataAnalyzeApi.Integration.Tests;

/// <summary>
/// Base class for integration tests with container setup and authentication.
/// </summary>
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
        postgresContainer = GetPostgreContainer();
        redisContainer = GetRedisContainer();
    }

    /// <summary>
    /// Starts containers, seeds database, and authenticates user.
    /// </summary>
    public async Task InitializeAsync()
    {
        await postgresContainer.StartAsync();
        await redisContainer.StartAsync();

        factory.UpdateConnectionStrings(
            postgresContainer.GetConnectionString(),
            redisContainer.GetConnectionString());

        await SeedDatabaseAsync();
        await AuthenticateAsync();
    }

    /// <summary>
    /// Stops and disposes containers after tests.
    /// </summary>
    public async Task DisposeAsync()
    {
        await postgresContainer.StopAsync();
        await redisContainer.StopAsync();
    }

    /// <summary>
    /// Loads configuration from files and environment variables.
    /// </summary>
    private static IConfigurationRoot GetConfiguration()
    {
        DotNetEnv.Env.Load(".env.test");

        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.test.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

    /// <summary>
    /// Sets up and return PostgreSQL container with configuration.
    /// </summary>
    private PostgreSqlContainer GetPostgreContainer()
    {
        var postgresConfig = configuration
            .GetSection("Postgres")
            .Get<PostgresConfig>()!;

        return new PostgreSqlBuilder()
            .WithImage("postgres")
            .WithPortBinding(5432, true)
            .WithDatabase(postgresConfig.Name)
            .WithUsername(postgresConfig.Username)
            .WithPassword(postgresConfig.Password)
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    /// <summary>
    /// Sets up and return Redis container with configuration.
    /// </summary>
    private static RedisContainer GetRedisContainer()
    {
        return new RedisBuilder()
            .WithImage("redis")
            .WithPortBinding(6379, true)
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
            .Build();
    }

    /// <summary>
    /// Applies migrations and seeds identity data into the database.
    /// </summary>
    private async Task SeedDatabaseAsync()
    {
        using var scope = factory.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

         var dbContext = serviceProvider.GetRequiredService<DataAnalyzeDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = serviceProvider.GetRequiredService<ILogger<IdentitySeeder>>();
        var identityOptions = serviceProvider.GetRequiredService<IOptions<IdentityConfig>>();

        var identitySeeder = new IdentitySeeder(
            userManager,
            roleManager,
            identityOptions,
            logger
        );

        await dbContext.Database.MigrateAsync();
        await identitySeeder.SeedAsync();
    }

    /// <summary>
    /// Authenticates test user and sets authorization header.
    /// </summary>
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
        authToken = authResult?.Token!;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
    }

    /// <summary>
    /// Creates a dataset from JSON file via API and returns its ID.
    /// </summary>
    protected async Task<long> CreateDatasetFromJsonAsync()
    {
        var jsonDataset = await File.ReadAllTextAsync("Data/technology-companies-analysis-2024.json");
        var datasetDto = JsonConvert.DeserializeObject<DatasetCreateDto>(jsonDataset);

        var response = await client.PostAsJsonAsync("/api/datasets", datasetDto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<Dataset>();
        Assert.NotNull(result);

        return result.Id;
    }
}
