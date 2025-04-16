using DataAnalyzeAPI.Models.Config;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Requests;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace DataAnalyzeAPI.Services.Cache;

public class SimilarityCacheService
{
    private readonly IDistributedCache cache;
    private readonly RedisConfig redisConfig;

    public SimilarityCacheService(
        IDistributedCache cache,
        IOptions<RedisConfig> redisConfigOptions)
    {
        this.cache = cache;
        redisConfig = redisConfigOptions.Value;
    }

    public async Task<SimilarityResult?> GetCachedResultAsync(
        long datasetId,
        SimilarityRequest? request)
    {
        var cacheKey = BuildCacheKey(datasetId, request);
        var cacheBytes = await cache.GetAsync(cacheKey);

        if (cacheBytes == null)
            return null;

        var cacheJson = Encoding.UTF8.GetString(cacheBytes);
        return JsonSerializer.Deserialize<SimilarityResult>(cacheJson);
    }

    public async Task CacheResultAsync(
        long datasetId,
        SimilarityRequest? request,
        SimilarityResult result)
    {
        var cacheKey = BuildCacheKey(datasetId, request);
        var resultJson = JsonSerializer.Serialize(result);
        var resultBytes = Encoding.UTF8.GetBytes(resultJson);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(redisConfig.DefaultCacheDurationMinutes)
        };

        await cache.SetAsync(cacheKey, resultBytes, cacheOptions);
    }

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
