using DataAnalyzeAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware();
app.MapControllers();

await app.InitializeDatabaseAsync();

app.Run();
