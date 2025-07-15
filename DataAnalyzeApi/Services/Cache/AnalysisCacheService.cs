using DataAnalyzeApi.Models.DTOs.Analysis;

namespace DataAnalyzeApi.Services.Cache;

public class AnalysisCacheService<TDto>(
    ICacheService cacheService
    )
    where TDto : BaseAnalysisResultDto
{
    private readonly ICacheService cacheService = cacheService;

    /// <summary>
    /// Retrieves the analysis result from cache based on the request and dataset.
    /// </summary>
    public async Task<TDto?> GetAsync(
        string analysisType,
        long datasetId,
        string requestHash)
    {
        var key = GenerateKey(analysisType, datasetId, requestHash);

        return await cacheService.GetAsync<TDto>(key);
    }

    /// <summary>
    /// Caches the analysis result for a given request and dataset.
    /// </summary>
    public async Task SetAsync(
        string analysisType,
        long datasetId,
        string requestHash,
        TDto result)
    {
        var key = GenerateKey(analysisType, datasetId, requestHash);

        await cacheService.SetAsync(key, result);
    }

    /// <summary>
    /// Builds a unique cache key based on analysis type, dataset ID and request hash.
    /// </summary>
    private static string GenerateKey(
        string analysisType,
        long datasetId,
        string requestHash)
    {
        return $"{analysisType}:{datasetId}:{requestHash}";
    }
}
