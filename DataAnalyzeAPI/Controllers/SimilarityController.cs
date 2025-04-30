using DataAnalyzeApi.Services.Analyse.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Requests;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Route("api/analyse/similarity")]
[Authorize(Policy = "UserOrAdmin")]
public class SimilarityController : ControllerBase
{
    private readonly DatasetService datasetService;
    private readonly SimilarityService similarityService;

    public SimilarityController(
        DatasetService datasetService,
        SimilarityService similarityService)
    {
        this.datasetService = datasetService;
        this.similarityService = similarityService;
    }

    /// <summary>
    /// Calculates  similarity results based on full pairwise comparison algorithm.
    /// Provides similarity score for each pair of objects.
    /// Returns a cached result if available; otherwise, performs a new similarity analysis.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">Similarity configuration parameters (optional)</param>
    /// <returns>An action result containing the similarity results or an error response</returns>
    [HttpPost("{datasetId}")]
    public async Task<IActionResult> CalculateSimilarity(
        long datasetId,
        [FromBody] SimilarityRequest? request)
    {
        var cachedResult = await similarityService.GetCachedResultAsync(datasetId, request);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var dataset = await datasetService.GetPreparedDatasetAsync(datasetId, request?.ParameterSettings);
        var similarityResult = await similarityService.PerformAnalysisAsync(dataset, request);

        return Ok(similarityResult);
    }
}
