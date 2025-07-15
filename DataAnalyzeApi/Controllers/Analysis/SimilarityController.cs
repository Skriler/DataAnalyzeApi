using DataAnalyzeApi.Attributes;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Services.Analysis.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Route("api/analysis/similarity")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class SimilarityController(
    DatasetService datasetService,
    SimilarityService similarityService,
    ILogger<SimilarityController> logger
    ) : ControllerBase
{
    protected readonly DatasetService datasetService = datasetService;
    protected readonly ILogger<SimilarityController> logger = logger;
    private readonly SimilarityService similarityService = similarityService;

    /// <summary>
    /// Calculates  similarity results based on full pairwise comparison algorithm.
    /// Provides similarity score for each pair of objects.
    /// Returns a cached result if available; otherwise, performs a new similarity analysis.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">Similarity configuration parameters (optional)</param>
    /// <returns>An action result containing the similarity results or an error response</returns>
    [HttpPost("{datasetId:long}")]
    [ProducesResponseType(typeof(SimilarityAnalysisResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SimilarityAnalysisResultDto>> CalculateSimilarity(
        [FromRoute][ValidId] long datasetId,
        [FromBody] SimilarityRequest? request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var storedResult = await similarityService.GetResultFromCacheOrDbAsync(
            datasetId,
            request);

        if (storedResult != null)
        {
            logger.LogInformation("Returning stored similarity result for dataset {DatasetId}", datasetId);
            return storedResult;
        }

        var dataset = await datasetService.GetPreparedDatasetAsync(
            datasetId,
            request?.ParameterSettings);

        var result = await similarityService.PerformAnalysisAsync(
            dataset,
            request);

        logger.LogInformation(
            "Successfully calculated similarity for dataset {DatasetId} with {PairCount} pairs",
            datasetId,
            result.Similarities.Count);

        return result;
    }
}
