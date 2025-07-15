using AutoMapper;
using DataAnalyzeApi.Attributes;
using DataAnalyzeApi.DAL.Repositories.Analysis;
using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers.Results;

[ApiController]
[Route("api/results/clustering")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class ClusteringAnalysisResultController(
    ClusteringAnalysisResultRepository repository,
    ClusteringEntityAnalysisMapper mapper
    ) : ControllerBase
{
    private readonly ClusteringAnalysisResultRepository repository = repository;
    private readonly ClusteringEntityAnalysisMapper mapper = mapper;

    /// <summary>
    /// Get all cluster analysis results.
    /// </summary>
    /// <returns>An action result containing the list of cluster analysis results or an error response</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClusteringAnalysisResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClusteringAnalysisResultDto>>> GetAll()
    {
        var results = await repository.GetAllAsync();

        return mapper.MapAnalysisResultList(results);
    }

    /// <summary>
    /// Get cluster analysis results by dataset id.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to retrieve cluster analysis results for</param>
    /// <returns>An action result containing the list of cluster analysis results or an error response</returns>
    [HttpGet("dataset/{datasetId:long}")]
    [ProducesResponseType(typeof(List<ClusteringAnalysisResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClusteringAnalysisResultDto>>> GetByDataset(
        [FromRoute][ValidId] long datasetId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var results = await repository.GetAllByDatasetAsync(datasetId);

        return mapper.MapAnalysisResultList(results);
    }

    /// <summary>
    /// Get cluster analysis results by dataset id and clustering algorithm.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to retrieve cluster analysis results for</param>
    /// <param name="algorithm">The clustering algorithm to filter by</param>
    /// <returns>An action result containing the list of cluster analysis results or an error response</returns>
    [HttpGet("dataset/{datasetId:long}/algorithm/{algorithm}")]
    [ProducesResponseType(typeof(List<ClusteringAnalysisResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClusteringAnalysisResultDto>>> GetByAlgorithm(
        [FromRoute][ValidId] long datasetId,
        [FromRoute] ClusteringAlgorithm algorithm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var results = await repository.GetByAlgorithmAsync(datasetId, algorithm);

        return mapper.MapAnalysisResultList(results);
    }
}
