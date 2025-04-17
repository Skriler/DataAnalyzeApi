using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Requests;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;
using System.Text.Json;

namespace DataAnalyzeAPI.Services.Cache;

public class SimilarityCacheService
{
    private readonly ICacheService cacheService;

    public SimilarityCacheService(ICacheService cacheService)
    {
        this.cacheService = cacheService;
    }

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
