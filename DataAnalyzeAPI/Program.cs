using DataAnalyzeAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();

app.ConfigureMiddleware();
app.MapControllers();

app.Run();
