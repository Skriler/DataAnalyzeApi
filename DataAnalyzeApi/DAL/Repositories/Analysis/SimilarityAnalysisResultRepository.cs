using DataAnalyzeApi.Models.Entities.Analysis.Similarity;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL.Repositories.Analysis;

public class SimilarityAnalysisResultRepository(DataAnalyzeDbContext context)
{
    private readonly DataAnalyzeDbContext context = context;

    /// <summary>
    /// Retrieves all similarity analysis results from the database.
    /// </summary>
    public async Task<List<SimilarityAnalysisResult>> GetAllAsync(bool trackChanges = false)
    {
        var query = GetSimilarityAnalysesWithIncludes();

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query
            .OrderByDescending(ca => ca.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all similarity analysis results for a specific dataset.
    /// </summary>
    public async Task<List<SimilarityAnalysisResult>> GetAllByDatasetAsync(
        long datasetId,
        bool trackChanges = false)
    {
        var query = GetSimilarityAnalysesWithIncludes()
            .Where(sa => sa.DatasetId == datasetId);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query
            .OrderByDescending(sa => sa.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a similarity analysis result by its unique request hash for a specific dataset.
    /// </summary>
    public async Task<SimilarityAnalysisResult?> GetByHashAsync(
        long datasetId,
        string requestHash,
        bool trackChanges = false)
    {
        var query = GetSimilarityAnalysesWithIncludes()
            .Where(sa => sa.DatasetId == datasetId && sa.RequestHash == requestHash);

        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Adds a new similarity analysis result to the database.
    /// </summary>
    public async Task AddAsync(SimilarityAnalysisResult similarityAnalysis)
    {
        context.SimilarityAnalyses.Add(similarityAnalysis);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Creates a query for similarity analyses with all necessary related data included.
    /// Includes similarity pairs and their associated objects (ObjectA and ObjectB) for complete data retrieval.
    /// </summary>
    private IQueryable<SimilarityAnalysisResult> GetSimilarityAnalysesWithIncludes()
    {
        //TODO: add parameter values if IncludeParameters is true

        return context.SimilarityAnalyses
            .Include(sa => sa.SimilarityPairs)
                .ThenInclude(sp => sp.ObjectA)
            .Include(sa => sa.SimilarityPairs)
                .ThenInclude(sp => sp.ObjectB);
    }
}
