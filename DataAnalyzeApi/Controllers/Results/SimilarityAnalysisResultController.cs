using DataAnalyzeApi.Attributes;
using DataAnalyzeApi.DAL.Repositories.Analysis;
using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers.Results;

[ApiController]
[Route("api/results/similarity")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class SimilarityAnalysisResultController(
    SimilarityAnalysisResultRepository repository,
    SimilarityEntityAnalysisMapper mapper
    ) : ControllerBase
{
    private readonly SimilarityAnalysisResultRepository repository = repository;
    private readonly SimilarityEntityAnalysisMapper mapper = mapper;

    /// <summary>
    /// Get all similarity analysis results.
    /// </summary>
    /// <returns>An action result containing the list of similarity analysis results or an error response</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<SimilarityAnalysisResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<SimilarityAnalysisResultDto>>> GetAll()
    {
        var results = await repository.GetAllAsync();

        return mapper.MapAnalysisResultList(results);
    }

    /// <summary>
    /// Get paginated similarity analysis results.
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>An action result containing paginated similarity analysis results</returns>
    [HttpGet("paged")]
    [ProducesResponseType(typeof(PaginationResult<SimilarityAnalysisResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginationResult<SimilarityAnalysisResultDto>>> GetPaged(
        [FromQuery] PaginationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var results = await repository.GetPagedAsync(request);

        return new PaginationResult<SimilarityAnalysisResultDto>
        {
            Data = mapper.MapAnalysisResultList(results.Data),
            TotalCount = results.TotalCount,
            PageNumber = results.PageNumber,
            PageSize = results.PageSize
        };
    }

    /// <summary>
    /// Get similarity analysis results by dataset id.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to retrieve similarity analysis results for</param>
    /// <returns>An action result containing the list of similarity analysis results or an error response</returns>
    [HttpGet("dataset/{datasetId:long}")]
    [ProducesResponseType(typeof(List<SimilarityAnalysisResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<SimilarityAnalysisResultDto>>> GetByDataset(
        [FromRoute][ValidId] long datasetId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var results = await repository.GetAllByDatasetAsync(datasetId);

        return mapper.MapAnalysisResultList(results);
    }
}
