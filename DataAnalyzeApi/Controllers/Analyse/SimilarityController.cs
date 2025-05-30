using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Results;
using DataAnalyzeApi.Services.Analyse.Core;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers.Analyse;

[Route("api/analyse/similarity")]
public class SimilarityController(
    DatasetService datasetService,
    SimilarityService similarityService,
    ILogger<SimilarityController> logger
    ) : BaseAnalyseController<SimilarityController>(datasetService, logger)
{
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
    [ProducesResponseType(typeof(SimilarityResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SimilarityResult>> CalculateSimilarity(
        [FromRoute] long datasetId,
        [FromBody] SimilarityRequest? request)
    {
        if (!TryValidateRequest(datasetId, out var errorResult))
        {
            return errorResult!;
        }

        var cachedResult = await similarityService.GetCachedResultAsync(datasetId, request);

        if (cachedResult != null)
        {
            logger.LogInformation("Returning cached similarity result for dataset {DatasetId}", datasetId);
            return cachedResult;
        }

        var dataset = await datasetService.GetPreparedDatasetAsync(datasetId, request?.ParameterSettings);

        var similarityResult = await similarityService.PerformAnalysisAsync(dataset, request);

        logger.LogInformation(
            "Successfully calculated similarity for dataset {DatasetId} with {PairCount} pairs",
            datasetId,
            similarityResult.Similarities.Count);

        return similarityResult;
    }
}
