using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL.Repositories.Analysis;

public class ClusterAnalysisResultRepository(DataAnalyzeDbContext context)
{
    private readonly DataAnalyzeDbContext context = context;

    /// <summary>
    /// Retrieves all cluster analysis results from the database.
    /// </summary>
    public async Task<List<ClusterAnalysisResult>> GetAllAsync(bool trackChanges = false)
    {
        var query = GetClusterAnalysesWithIncludes();

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query
            .OrderByDescending(ca => ca.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all cluster analysis results for a specific dataset.
    /// </summary>
    public async Task<List<ClusterAnalysisResult>> GetAllByDatasetAsync(
        long datasetId,
        bool trackChanges = false)
    {
        var query = GetClusterAnalysesWithIncludes()
            .Where(ca => ca.DatasetId == datasetId);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query
            .OrderByDescending(ca => ca.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a cluster analysis result by its unique request hash for a specific dataset.
    /// </summary>
    public async Task<ClusterAnalysisResult?> GetByHashAsync(
        long datasetId,
        string requestHash,
        bool trackChanges = false)
    {
        var query = GetClusterAnalysesWithIncludes()
            .Where(ca => ca.DatasetId == datasetId && ca.RequestHash == requestHash);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves all cluster analysis results for a specific dataset filtered by clustering algorithm.
    /// </summary>
    public async Task<List<ClusterAnalysisResult>> GetByAlgorithmAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        bool trackChanges = false)
    {
        var query = GetClusterAnalysesWithIncludes()
            .Where(ca => ca.DatasetId == datasetId && ca.Algorithm == algorithm);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query
            .OrderByDescending(ca => ca.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new cluster analysis result to the database.
    /// </summary>
    public async Task AddAsync(ClusterAnalysisResult clusterAnalysis)
    {
        context.ClusterAnalyses.Add(clusterAnalysis);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Creates a query for cluster analyses with all necessary related data included.
    /// Includes clusters and their associated objects for complete data retrieval.
    /// </summary>
    private IQueryable<ClusterAnalysisResult> GetClusterAnalysesWithIncludes()
    {
        //TODO: add parameter values if IncludeParameters is true

        return context.ClusterAnalyses
            .Include(ca => ca.Clusters)
                .ThenInclude(c => c.Objects);
    }
}
