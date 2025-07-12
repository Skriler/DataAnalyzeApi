using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL;

public class DataAnalyzeDbContext(
    DbContextOptions<DataAnalyzeDbContext> options
    ) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Dataset> Datasets { get; set; }

    public DbSet<DataObject> DataObjects { get; set; }

    public DbSet<Parameter> Parameters { get; set; }

    public DbSet<ParameterValue> ParameterValues { get; set; }

    public DbSet<SimilarityAnalysisResult> SimilarityAnalyses { get; set; }

    public DbSet<SimilarityPair> SimilarityPairs { get; set; }

    public DbSet<ClusterAnalysisResult> ClusterAnalyses { get; set; }

    public DbSet<Cluster> Clusters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureEntityRelationships(modelBuilder);
        ConfigureEntityProperties(modelBuilder);
    }

    /// <summary>
    /// Configures entity relationships and foreign keys with cascade delete behavior
    /// </summary>
    private void ConfigureEntityRelationships(ModelBuilder modelBuilder)
    {
        // Configure inheritance strategy for AnalysisResult
        modelBuilder.Entity<AnalysisResult>().UseTpcMappingStrategy();
        modelBuilder.Entity<SimilarityAnalysisResult>().ToTable("SimilarityAnalysisResults");
        modelBuilder.Entity<ClusterAnalysisResult>().ToTable("ClusterAnalysisResults");

        // Main entities relationships - cascade delete enabled for all child entities
        modelBuilder.Entity<Dataset>(entity =>
        {
            entity.HasMany(d => d.Objects)
                .WithOne(o => o.Dataset)
                .HasForeignKey(o => o.DatasetId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Parameters)
                .WithOne(type => type.Dataset)
                .HasForeignKey(o => o.DatasetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DataObject>()
            .HasMany(o => o.Values)
            .WithOne(v => v.Object)
            .HasForeignKey(v => v.ObjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Analysis result entities relationships - cascade delete enabled for all child entities
        modelBuilder.Entity<SimilarityAnalysisResult>()
            .HasOne(sa => sa.Dataset)
            .WithMany()
            .HasForeignKey(sa => sa.DatasetId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SimilarityPair>()
            .HasOne(sp => sp.SimilarityAnalysisResult)
            .WithMany(sa => sa.SimilarityPairs)
            .HasForeignKey(sp => sp.SimilarityAnalysisResultId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SimilarityPair>(entity =>
        {
            entity.HasOne(sp => sp.ObjectA)
                .WithMany()
                .HasForeignKey(sp => sp.ObjectAId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(sp => sp.ObjectB)
                .WithMany()
                .HasForeignKey(sp => sp.ObjectBId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ClusterAnalysisResult>()
            .HasOne(ca => ca.Dataset)
            .WithMany()
            .HasForeignKey(ca => ca.DatasetId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cluster>()
            .HasOne(c => c.ClusterAnalysisResult)
            .WithMany(ca => ca.Clusters)
            .HasForeignKey(c => c.ClusterAnalysisResultId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cluster>()
            .HasMany(c => c.Objects)
            .WithMany()
            .UsingEntity("ClusterDataObjects");
    }

    /// <summary>
    /// Configures entity properties and value conversions
    /// </summary>
    private void ConfigureEntityProperties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parameter>()
            .Property(p => p.Type)
            .HasConversion<int>();
    }
}
