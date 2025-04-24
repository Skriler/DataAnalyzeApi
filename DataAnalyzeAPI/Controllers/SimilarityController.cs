using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataAnalyzeAPI.Services.Cache;
using DataAnalyzeAPI.Services.Analyse.Core;

namespace DataAnalyzeAPI.Controllers;

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
        var cachedResult = await similarityService.GetCachedResultAsync(datasetId, request);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var dataset = await datasetService.GetPreparedDatasetAsync(datasetId, request?.ParameterSettings);
        var similarityResult = await similarityService.CalculateSimilarityAsync(dataset, request);

        return Ok(similarityResult);
    }
}
