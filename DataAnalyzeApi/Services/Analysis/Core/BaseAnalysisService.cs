using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.Entities.Analysis;
using DataAnalyzeApi.Services.Analysis.Results;
using DataAnalyzeApi.Services.Cache;

namespace DataAnalyzeApi.Services.Analysis.Core;

/// <summary>
/// Base class for analysis services.
/// </summary>
public abstract class BaseAnalysisService<TRequest, TEntity, TDto, TResultService>(
    AnalysisCacheService<TDto> cacheService,
    TResultService resultService,
    string cachePrefix
    )
    where TRequest : BaseAnalysisRequest
    where TEntity : AnalysisResult
    where TDto : BaseAnalysisResultDto
    where TResultService : BaseAnalysisResultService<TEntity, TDto>
{
    protected readonly AnalysisCacheService<TDto> cacheService = cacheService;
    protected readonly TResultService resultService = resultService;
    protected readonly string cachePrefix = cachePrefix;

    /// <summary>
    /// Returns result from cache or database by request hash. Caches DB result if found.
    /// </summary>
    public async Task<TDto?> GetResultFromCacheOrDbAsync(long datasetId, TRequest? request)
    {
        var requestHash = GenerateRequestHash(request);

        var cachedResult = await cacheService.GetAsync(
            cachePrefix,
            datasetId,
            requestHash);

        if (cachedResult != null)
            return cachedResult;

        var dbResult = await resultService.GetDtoByHashAsync(
            datasetId,
            requestHash,
            request?.IncludeParameters ?? false);

        if (dbResult != null)
        {
            await cacheService.SetAsync(cachePrefix, datasetId, requestHash, dbResult);
            return dbResult;
        }

        return null;
    }

    /// <summary>
    /// Generates a hash string based on the request object for use in cache key.
    /// </summary>
    public string GenerateRequestHash(TRequest? request)
    {
        if (request == null)
            return "default";

        var requestType = request.GetType();
        var requestJson = JsonSerializer.Serialize(request, requestType);
        var input = $"{requestType.Name}:{requestJson}";

        var hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hashBytes);
    }
}
