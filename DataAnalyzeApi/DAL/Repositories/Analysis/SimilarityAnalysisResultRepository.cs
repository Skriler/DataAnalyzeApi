using DataAnalyzeApi.Models.Entities.Analysis.Similarity;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.DAL.Repositories.Analysis;

public class SimilarityAnalysisResultRepository(DataAnalyzeDbContext context)
     : BaseAnalysisRepository<SimilarityAnalysisResult>(context)
{
    /// <summary>
    /// Get a DbSet for a specific analysis type.
    /// </summary>
    protected override DbSet<SimilarityAnalysisResult> GetDbSet() => context.SimilarityAnalysisResults;

    /// <summary>
    /// Creates a query for similarity analysis with all necessary related data included.
    /// Includes similarity pairs and their associated objects (ObjectA and ObjectB) for complete data retrieval.
    /// Adds parameter values if flag is true.
    /// </summary>
    protected override IQueryable<SimilarityAnalysisResult> GetAnalysisResultsWithIncludes(
        bool includeParameters = false)
    {
        var objectAInclude = includeParameters
            ? "SimilarityPairs.ObjectA.Values.Parameter"
            : "SimilarityPairs.ObjectA";

        var objectBInclude = includeParameters
            ? "SimilarityPairs.ObjectB.Values.Parameter"
            : "SimilarityPairs.ObjectB";

        return context.SimilarityAnalysisResults
            .Include(objectAInclude)
            .Include(objectBInclude);
    }
}
