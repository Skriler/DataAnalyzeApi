using AutoMapper;
using DataAnalyzeAPI.Services.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/analyse")]
public class AnalyseController : Controller
{
    private readonly DatasetRepository repository;

    public AnalyseController(DatasetRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Get similarity results based on full pairwise comparison algorithm.
    /// </summary>
    [HttpPost("similarity/{datasetId}")]
    public async Task<IActionResult> CalculateSimilarity(long datasetId)
    {
        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
        {
            return NotFound($"Dataset with ID {datasetId} not found");
        }


    }
}
