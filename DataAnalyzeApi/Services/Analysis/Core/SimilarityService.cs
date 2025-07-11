using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Services.Analysis.Comparers;
using DataAnalyzeApi.Services.Cache;

namespace DataAnalyzeApi.Services.Analysis.Core;

public class SimilarityService(
    DatasetService datasetService,
    AnalysisMapper analysisMapper,
    SimilarityComparer comparer,
    SimilarityCacheService cacheService
    ) : BaseAnalysisService(datasetService, analysisMapper)
{
    private readonly SimilarityComparer comparer = comparer;
    private readonly SimilarityCacheService cacheService = cacheService;

    /// <summary>
    /// Performs similarity analysis on the given dataset and returns the result.
    /// </summary>
    public async Task<SimilarityResult> PerformAnalysisAsync(DatasetModel dataset, SimilarityRequest? request)
    {
        var similarities = comparer.CompareAllObjects(dataset);

        var includeParameters = request?.IncludeParameters ?? false;
        var similaritiesDto = analysisMapper.MapSimilarityPairDtoList(similarities, includeParameters);

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
