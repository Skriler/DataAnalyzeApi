using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Results;
using DataAnalyzeApi.Services.Analyse.Comparers;
using DataAnalyzeApi.Services.Cache;

namespace DataAnalyzeApi.Services.Analyse.Core;

public class SimilarityService : BaseAnalysisService
{
    private readonly SimilarityComparer comparer;
    private readonly SimilarityCacheService cacheService;

    public SimilarityService(
        DatasetService datasetService,
        AnalysisMapper analysisMapper,
        SimilarityComparer comparer,
        SimilarityCacheService cacheService)
        : base(datasetService, analysisMapper)
    {
        this.comparer = comparer;
        this.cacheService = cacheService;
    }

    /// <summary>
    /// Performs similarity analysis on the given dataset and returns the result.
    /// </summary>
    public async Task<SimilarityResult> PerformAnalysisAsync(DatasetModel dataset, SimilarityRequest? request)
    {
        var similarities = comparer.CalculateSimilarity(dataset);

        var includeParameters = request?.IncludeParameters ?? false;
        var similaritiesDto = analysisMapper.MapSimilarityPairList(similarities, includeParameters);

        var similarityResult = new SimilarityResult()
        {
            DatasetId = dataset.Id,
            Similarities = similaritiesDto,
        };

        await cacheService.CacheResultAsync(dataset.Id, request, similarityResult);

        return similarityResult;
    }

    /// <summary>
    /// Retrieves a cached similarity result for the given dataset and request, if available.
    /// </summary>
    public async Task<SimilarityResult?> GetCachedResultAsync(long datasetId, SimilarityRequest? request)
    {
        return await cacheService.GetCachedResultAsync(datasetId, request);
    }
}
