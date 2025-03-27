using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity;
using DataAnalyzeAPI.Services.Analyse;
using DataAnalyzeAPI.Services.DAL;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/analyse")]
public class AnalyseController : Controller
{
    private readonly DatasetRepository repository;
    private readonly DatasetSettingsMapper datasetSettingsMapper;
    private readonly SimilarityComparer comparer;

    public AnalyseController(
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
    [HttpPost("similarity/{datasetId}")]
    public async Task<IActionResult> CalculateSimilarity(
        long datasetId,
        [FromBody] SimilarityRequest? request)
    {
        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
        {
            return NotFound($"Dataset with ID {datasetId} not found.");
        }

        var mappedDataset = datasetSettingsMapper.MapObjects(dataset, request?.ParameterSettings);
        var similarities = comparer.CalculateSimilarity(mappedDataset);

        var similarityResult = new SimilarityResult()
        {
            DatasetId = datasetId,
            Similarities = similarities,
        };

        return Ok(similarityResult);
    }
}
