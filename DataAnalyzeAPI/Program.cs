using DataAnalyzeApi.Extensions;

// Load environment variables
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddEnvironmentVariables();

// Dependency Injection
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Middleware & Routing
app.ConfigureMiddleware();
app.MapControllers();

// Database seeding (except for Testing environment)
if (!app.Environment.IsEnvironment("Testing"))
{
    await app.InitializeDatabaseAsync();
}

app.Run();

public partial class Program;