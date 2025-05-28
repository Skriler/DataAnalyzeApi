using DataAnalyzeApi.DAL.Seeders;
using DataAnalyzeApi.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace DataAnalyzeApi.Extensions.Core;

/// <summary>
/// Contains extension methods for application setup and middleware configuration.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Loads environment variables from a .env file based on the current environment.
    /// </summary>
    public static void LoadEnvironmentVariables(this WebApplicationBuilder builder)
    {
        var environmentName = builder.Environment.EnvironmentName ?? "Development";
        var envFile = environmentName == "Testing" ? ".env.test" : ".env";

        if (File.Exists(envFile))
        {
            DotNetEnv.Env.Load(envFile);
        }

        builder.Configuration.AddEnvironmentVariables();
    }

    /// <summary>
    /// Configures the application middleware pipeline.
    /// </summary>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        ConfigureSwagger(app);

        app.UseErrorHandlingMiddleware()
            .UseHttpsRedirection()
            .UseAuthorization();

        return app;
    }

    /// <summary>
    /// Initializes the database with seed data.
    /// </summary>
    public static async Task<IApplicationBuilder> InitializeDatabaseAsync(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
            await seeder.SeedAsync();
        }

        return app;
    }

    /// <summary>
    /// Configures development-specific services.
    /// </summary>
    private static void ConfigureSwagger(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Data Analyze API v1");
            opt.RoutePrefix = "swagger";
        });
    }
}
