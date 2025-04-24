using DataAnalyzeAPI.Extensions;
using DataAnalyzeAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddScoped<DataAnalyzeExceptionFilter>();
var app = builder.Build();

app.ConfigureMiddleware();
app.MapControllers();

await app.InitializeDatabaseAsync();

app.Run();
