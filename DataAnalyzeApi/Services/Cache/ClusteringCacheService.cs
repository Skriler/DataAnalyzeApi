using System.Text.Json;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Services.Cache;

public class ClusteringCacheService(ICacheService cacheService)
{
    private readonly ICacheService cacheService = cacheService;

    public async Task<ClusterAnalysisResultDto?> GetCachedResultAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        BaseClusteringRequest request)
    {
        var cacheKey = BuildCacheKey(datasetId, algorithm, request);
        return await cacheService.GetAsync<ClusterAnalysisResultDto>(cacheKey);
    }

    public async Task CacheResultAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        BaseClusteringRequest request,
        ClusterAnalysisResultDto result)
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
