using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Requests;
using DataAnalyzeAPI.Services.Analyse.Comparers;
using DataAnalyzeAPI.Services.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/analyse/similarity")]
public class SimilarityController : Controller
{
    private readonly DatasetRepository repository;
    private readonly DatasetSettingsMapper datasetSettingsMapper;
    private readonly SimilarityComparer comparer;

    public SimilarityController(
        DatasetRepository repository,
        DatasetSettingsMapper datasetSettingsMapper,
        SimilarityComparer comparer)
    {
        this.repository = repository;
        this.datasetSettingsMapper = datasetSettingsMapper;
        this.comparer = comparer;
    }

    /// <summary>
    /// Get similarity results based on full pairwise comparison algorithm.
    /// </summary>
    [HttpPost("{datasetId}")]
    public async Task<IActionResult> CalculateSimilarity(
        long datasetId,
        [FromBody] SimilarityRequest? request)
    {
        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
        {
            return NotFound($"Dataset with ID {datasetId} not found.");
        }

        var mappedDataset = datasetSettingsMapper.Map(dataset, request?.ParameterSettings);
        var similarities = comparer.CalculateSimilarity(mappedDataset);

        var similarityResult = new SimilarityResult()
        {
            DatasetId = datasetId,
            Similarities = similarities,
        };

        return Ok(similarityResult);
    }
}
