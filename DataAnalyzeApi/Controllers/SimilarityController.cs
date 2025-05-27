using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Results;
using DataAnalyzeApi.Services.Analyse.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Route("api/analyse/similarity")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class SimilarityController(
    DatasetService datasetService,
    SimilarityService similarityService,
    ILogger<SimilarityController> logger
    ) : ControllerBase
{
    private readonly DatasetService datasetService = datasetService;
    private readonly SimilarityService similarityService = similarityService;
    private readonly ILogger<SimilarityController> logger = logger;

    /// <summary>
    /// Calculates  similarity results based on full pairwise comparison algorithm.
    /// Provides similarity score for each pair of objects.
    /// Returns a cached result if available; otherwise, performs a new similarity analysis.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">Similarity configuration parameters (optional)</param>
    /// <returns>An action result containing the similarity results or an error response</returns>
    [HttpPost("{datasetId:long}")]
    [ProducesResponseType(typeof(SimilarityResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CalculateSimilarity(
        long datasetId,
        [FromBody] SimilarityRequest? request)
    {
        if (datasetId <= 0)
        {
            return BadRequest("Invalid dataset ID");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cachedResult = await similarityService.GetCachedResultAsync(datasetId, request);

        if (cachedResult != null)
        {
            logger.LogInformation("Returning cached similarity result for dataset {DatasetId}", datasetId);
            return Ok(cachedResult);
        }

        var dataset = await datasetService.GetPreparedDatasetAsync(datasetId, request?.ParameterSettings);

        var similarityResult = await similarityService.PerformAnalysisAsync(dataset, request);

        logger.LogInformation(
            "Successfully calculated similarity for dataset {DatasetId} with {PairCount} pairs",
            datasetId,
            similarityResult.Similarities.Count);

        return Ok(similarityResult);
    }
}
