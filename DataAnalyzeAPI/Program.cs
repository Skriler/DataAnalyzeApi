using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Services.Analyse.Clusterers;
using DataAnalyzeAPI.Services.Analyse.Comparers;
using DataAnalyzeAPI.Services.DAL;
using DataAnalyzeAPI.Services.Normalizers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataAnalyzeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(DatasetProfile));

builder.Services.AddScoped<DatasetRepository>();
builder.Services.AddScoped<DatasetSettingsMapper>();
builder.Services.AddScoped<DatasetNormalizer>();

builder.Services.AddScoped<SimilarityComparer>();
builder.Services.AddTransient<ICompare, NormalizedValueComparer>();

builder.Services.AddScoped<ClustererFactory>();
builder.Services.AddScoped<KMeansClusterer>();
builder.Services.AddScoped<DBSCANClusterer>();
builder.Services.AddScoped<AgglomerativeClusterer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Data Analyze API v1");
        opt.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
