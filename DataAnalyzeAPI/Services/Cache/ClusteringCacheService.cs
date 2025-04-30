using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Cache;
using System.Text.Json;

public class ClusteringCacheService
{
    private readonly ICacheService cacheService;

    public ClusteringCacheService(ICacheService cacheService)
    {
        this.cacheService = cacheService;
    }

    public async Task<ClusteringResult?> GetCachedResultAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        BaseClusteringRequest request)
    {
        var cacheKey = BuildCacheKey(datasetId, algorithm, request);
        return await cacheService.GetAsync<ClusteringResult>(cacheKey);
    }

    public async Task CacheResultAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        BaseClusteringRequest request,
        ClusteringResult result)
    {
        var cacheKey = BuildCacheKey(datasetId, algorithm, request);
        await cacheService.SetAsync(cacheKey, result);
    }

    /// <summary>
    /// Builds a unique cache key based on dataset ID, algorithm, and clustering request.
    /// </summary>
    private static string BuildCacheKey(long datasetId, ClusterAlgorithm algorithm, BaseClusteringRequest request)
    {
        var requestType = request.GetType().Name;
        var requestJson = JsonSerializer.Serialize(request, request.GetType());

        return $"clustering:{datasetId}:{algorithm}:{requestType}:{requestJson.GetHashCode()}";
    }
}