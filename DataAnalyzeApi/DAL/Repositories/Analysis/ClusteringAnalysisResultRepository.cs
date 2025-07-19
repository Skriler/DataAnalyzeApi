using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL.Repositories.Analysis;

public class ClusteringAnalysisResultRepository(DataAnalyzeDbContext context)
     : BaseAnalysisRepository<ClusteringAnalysisResult>(context)
{
    /// <summary>
    /// Get a DbSet for a specific analysis type.
    /// </summary>
    protected override DbSet<ClusteringAnalysisResult> GetDbSet() => context.ClusteringAnalysisResults;

    /// <summary>
    /// Creates a query for cluster analysis with all necessary related data included.
    /// Includes clusters and their associated objects for complete data retrieval.
    /// </summary>
    protected override IQueryable<ClusteringAnalysisResult> GetAnalysisResultsWithIncludes(
        bool includeParameters = false)
    {
        var objectsInclude = includeParameters
            ? "Clusters.Objects.Values.Parameter"
            : "Clusters.Objects";

        return context.ClusteringAnalysisResults
            .Include(objectsInclude)
            .Include(car => car.ObjectCoordinates);
    }

    /// <summary>
    /// Retrieves all cluster analysis results for a specific dataset filtered by clustering algorithm.
    /// Adds parameter values if flag is true.
    /// </summary>
    public async Task<List<ClusteringAnalysisResult>> GetByAlgorithmAsync(
        long datasetId,
        ClusteringAlgorithm algorithm,
        bool includeParameters = false,
        bool trackChanges = false)
    {
        var query = GetAnalysisResultsWithIncludes(includeParameters)
            .Where(ca => ca.DatasetId == datasetId && ca.Algorithm == algorithm);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query
            .OrderByDescending(ca => ca.CreatedAt)
            .ToListAsync();
    }
}
