namespace DataAnalyzeAPI.Services.Cache;

public interface ICacheService
{
    /// <summary>
    /// Retrieves a cached value by key.
    /// </summary>
    Task<T?> GetAsync<T>(string cacheKey);

    /// <summary>
    /// Stores a value in the cache with optional expiration.
    /// </summary>
    Task SetAsync<T>(string cacheKey, T value, TimeSpan? expiration = null);
}
