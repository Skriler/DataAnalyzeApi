using DataAnalyzeAPI.DAL.Seeders;
using DataAnalyzeAPI.Middlewares;

namespace DataAnalyzeAPI.Extensions;

/// <summary>
/// Contains extension methods for application setup and middleware configuration.
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Configures the application middleware pipeline.
    /// </summary>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            ConfigureDevEnvironment(app);
        }

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
    private static void ConfigureDevEnvironment(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Data Analyze API v1");
            opt.RoutePrefix = string.Empty;
        });
    }
}
