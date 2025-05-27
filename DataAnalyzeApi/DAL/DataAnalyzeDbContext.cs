using DataAnalyzeApi.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL;

public class DataAnalyzeDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Dataset> Datasets { get; set; }

    public DbSet<DataObject> DataObjects { get; set; }

    public DbSet<Parameter> Parameters { get; set; }

    public DbSet<ParameterValue> ParameterValues { get; set; }

    public DataAnalyzeDbContext(DbContextOptions<DataAnalyzeDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Dataset>(entity =>
        {
            entity.HasMany(d => d.Objects)
                .WithOne(o => o.Dataset)
                .HasForeignKey(o => o.DatasetId);

            entity.HasMany(d => d.Parameters)
                .WithOne(type => type.Dataset)
                .HasForeignKey(o => o.DatasetId);
        });

        modelBuilder.Entity<DataObject>()
            .HasMany(o => o.Values)
            .WithOne(v => v.Object)
            .HasForeignKey(v => v.ObjectId);

        modelBuilder.Entity<Parameter>()
            .Property(p => p.Type)
            .HasConversion<int>();
    }
}
