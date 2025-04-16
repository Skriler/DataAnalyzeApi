using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Requests;
using DataAnalyzeAPI.Services.Analyse.Comparers;
using DataAnalyzeAPI.Services.DAL;
using Microsoft.AspNetCore.Mvc;
using DataAnalyzeAPI.Services.Cache;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/analyse/similarity")]
public class SimilarityController : Controller
{
    private readonly DatasetRepository repository;

    private readonly DatasetSettingsMapper datasetSettingsMapper;
    private readonly AnalysisMapper analysisMapper;

    private readonly SimilarityComparer comparer;

    private readonly SimilarityCacheService cacheService;

    public SimilarityController(
        DatasetRepository repository,
        DatasetSettingsMapper datasetSettingsMapper,
        AnalysisMapper analysisMapper,
        SimilarityComparer comparer,
        SimilarityCacheService cacheService)
    {
        this.repository = repository;
        this.datasetSettingsMapper = datasetSettingsMapper;
        this.analysisMapper = analysisMapper;
        this.comparer = comparer;
        this.cacheService = cacheService;
    }

    /// <summary>
    /// Get similarity results based on full pairwise comparison algorithm.
    /// This method computes the pairwise similarity between objects in the dataset,
    /// using the configured comparison algorithm. It returns the similarity score
    /// for each pair of objects based on the selected settings.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">Similarity configuration parameters (optional)</param>
    /// <returns>An action result containing the similarity results or an error response</returns>
    [HttpPost("{datasetId}")]
    public async Task<IActionResult> CalculateSimilarity(
        long datasetId,
        [FromBody] SimilarityRequest? request)
    {
        var cachedResult = await cacheService.GetCachedResultAsync(datasetId, request);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
        {
            return NotFound($"Dataset with ID {datasetId} not found.");
        }

        var mappedDataset = datasetSettingsMapper.Map(dataset, request?.ParameterSettings);
        var similarities = comparer.CalculateSimilarity(mappedDataset);

        var includeParameters = request?.IncludeParameters ?? false;
        var similaritiesDto = analysisMapper.MapSimilarityPairList(similarities, includeParameters);

        var similarityResult = new SimilarityResult()
        {
            DatasetId = datasetId,
            Similarities = similaritiesDto,
        };

        await cacheService.CacheResultAsync(datasetId, request, similarityResult);

        return Ok(similarityResult);
    }
}
