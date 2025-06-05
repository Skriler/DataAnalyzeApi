using System.Text.Json;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

namespace DataAnalyzeApi.Services.Cache;

public class SimilarityCacheService(ICacheService cacheService)
{
    private readonly ICacheService cacheService = cacheService;

    public async Task<SimilarityResult?> GetCachedResultAsync(
        long datasetId,
        SimilarityRequest? request)
    {
        var cacheKey = BuildCacheKey(datasetId, request);
        return await cacheService.GetAsync<SimilarityResult>(cacheKey);
    }

    public async Task CacheResultAsync(
        long datasetId,
        SimilarityRequest? request,
        SimilarityResult result)
    {
        var cacheKey = BuildCacheKey(datasetId, request);
        await cacheService.SetAsync(cacheKey, result);
    }

    /// <summary>
    /// Builds a unique cache key based on dataset ID and similarity request.
    /// </summary>
    private static string BuildCacheKey(
        long datasetId,
        SimilarityRequest? request)
    {
        if (request == null)
            return $"similarity:{datasetId}:default";

        var requestJson = JsonSerializer.Serialize(request);
        return $"similarity:{datasetId}:{requestJson.GetHashCode()}";
    }
}
