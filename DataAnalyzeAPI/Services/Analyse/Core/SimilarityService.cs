using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Requests;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;
using DataAnalyzeAPI.Models.Enums;
using DataAnalyzeAPI.Services.Analyse.Comparers;
using DataAnalyzeAPI.Services.Cache;

namespace DataAnalyzeAPI.Services.Analyse.Core;

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

    public async Task<SimilarityResult> CalculateSimilarityAsync(DatasetModel dataset, SimilarityRequest? request)
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

    public async Task<SimilarityResult?> GetCachedResultAsync(long datasetId, SimilarityRequest? request)
    {
        return await cacheService.GetCachedResultAsync(datasetId, request);
    }
}
