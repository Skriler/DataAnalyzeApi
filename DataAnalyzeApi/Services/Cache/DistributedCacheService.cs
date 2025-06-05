using System.Text;
using System.Text.Json;
using DataAnalyzeApi.Models.Config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DataAnalyzeApi.Services.Cache;

public class DistributedCacheService(
    IDistributedCache cache,
    IOptions<RedisConfig> redisConfigOptions
    ) : ICacheService
{
    private readonly IDistributedCache cache = cache;
    private readonly RedisConfig redisConfig = redisConfigOptions.Value;

    /// <summary>
    /// Retrieves a cached value from the distributed cache.
    /// </summary>
    public async Task<T?> GetAsync<T>(string cacheKey)
    {
        var cacheBytes = await cache.GetAsync(cacheKey);

        if (cacheBytes == null)
            return default;

        var cacheJson = Encoding.UTF8.GetString(cacheBytes);
        return JsonSerializer.Deserialize<T>(cacheJson);
    }

    /// <summary>
    /// Stores a value in the distributed cache with optional expiration.
    /// </summary>
    public async Task SetAsync<T>(string cacheKey, T value, TimeSpan? expiration = null)
    {
        var resultJson = JsonSerializer.Serialize(value);
        var resultBytes = Encoding.UTF8.GetBytes(resultJson);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ??
                TimeSpan.FromMinutes(redisConfig.DefaultCacheDurationMinutes)
        };

        await cache.SetAsync(cacheKey, resultBytes, cacheOptions);
    }
}
