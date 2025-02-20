using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Services.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataAnalyzeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(DatasetProfile));

builder.Services.AddScoped<DatasetRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
