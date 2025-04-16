using DataAnalyzeAPI.Models.Config;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeAPI.Models.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

public class ClusteringCacheService
{
    private readonly IDistributedCache cache;
    private readonly RedisConfig redisConfig;

    public ClusteringCacheService(
        IDistributedCache cache,
        IOptions<RedisConfig> redisConfigOptions)
    {
        this.cache = cache;
        redisConfig = redisConfigOptions.Value;
    }

    public async Task<ClusteringResult?> GetCachedResultAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        BaseClusteringRequest request)
    {
        var cacheKey = BuildCacheKey(datasetId, algorithm, request);
        var cacheBytes = await cache.GetAsync(cacheKey);

        if (cacheBytes == null)
            return null;

        var cacheJson = Encoding.UTF8.GetString(cacheBytes);
        return JsonSerializer.Deserialize<ClusteringResult>(cacheJson);
    }

    public async Task CacheResultAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        BaseClusteringRequest request,
        ClusteringResult result)
    {
        var cacheKey = BuildCacheKey(datasetId, algorithm, request);
        var resultJson = JsonSerializer.Serialize(result);

        var resultBytes = Encoding.UTF8.GetBytes(resultJson);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(redisConfig.DefaultCacheDurationMinutes)
        };

        await cache.SetAsync(cacheKey, resultBytes, cacheOptions);
    }

    private static string BuildCacheKey(long datasetId, ClusterAlgorithm algorithm, BaseClusteringRequest request)
    {
        var requestType = request.GetType().Name;
        var requestJson = JsonSerializer.Serialize(request, request.GetType());

        return $"clustering:{datasetId}:{algorithm}:{requestType}:{requestJson.GetHashCode()}";
    }
}