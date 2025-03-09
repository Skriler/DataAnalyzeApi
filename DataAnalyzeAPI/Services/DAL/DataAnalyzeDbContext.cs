using DataAnalyzeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeAPI.Services.DAL;

public class DataAnalyzeDbContext : DbContext
{
    public DbSet<Dataset> Datasets { get; set; }

    public DbSet<DataObject> DataObjects { get; set; }

    public DbSet<Parameter> Parameters { get; set; }

    public DbSet<ParameterValue> ParameterValues { get; set; }

    public DataAnalyzeDbContext(DbContextOptions<DataAnalyzeDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
