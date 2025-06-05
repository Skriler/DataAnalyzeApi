using AutoMapper;
using DataAnalyzeApi.Attributes;
using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.DTOs.Dataset.Read;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Services.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Route("api/datasets")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class DatasetController(
    DatasetRepository repository,
    IMapper mapper,
    DatasetValidator validator,
    ILogger<DatasetController> logger
    ) : ControllerBase
{
    private readonly DatasetRepository repository = repository;
    private readonly IMapper mapper = mapper;
    private readonly DatasetValidator validator = validator;
    private readonly ILogger<DatasetController> logger = logger;

    /// <summary>
    /// Get all datasets.
    /// </summary>
    /// <returns>An action result containing the list of datasets or an error response</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Dataset>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<DatasetDto>>> GetAll()
    {
        var datasets = await repository.GetAllAsync();

        return mapper.Map<List<DatasetDto>>(datasets);
    }

    /// <summary>
    /// Get dataset by id.
    /// </summary>
    /// <param name="id">The ID of the dataset to retrieve</param>
    /// <returns>An action result containing the dataset DTO or a NotFound response</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(DatasetCreateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DatasetDto>> GetById(
        [FromRoute][ValidId] long id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dataset = await repository.GetByIdAsync(id);

        if (dataset == null)
        {
            return NotFound($"Dataset with ID {id} not found");
        }

        return mapper.Map<DatasetDto>(dataset);
    }

    /// <summary>
    /// Create new dataset.
    /// </summary>
    /// <param name="dto">The dataset creation details (DatasetCreateDto)</param>
    /// <returns>An action result indicating the outcome of the creation process</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Dataset), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Dataset>> Create(
        [FromBody] DatasetCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationResult = validator.ValidateDataset(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors });
        }

        var dataset = mapper.Map<Dataset>(dto);
        await repository.AddAsync(dataset);

        logger.LogInformation(
            "Dataset created successfully with ID {DatasetId}",
            dataset.Id);

        var result = mapper.Map<DatasetDto>(dataset);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Delete dataset by id.
    /// </summary>
    /// <param name="id">The ID of the dataset to delete</param>
    /// <returns>An action result indicating the outcome of the deletion process</returns>
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OnlyAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(
        [FromRoute][ValidId] long id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dataset = await repository.GetByIdAsync(id);

        if (dataset == null)
        {
            return NotFound($"Dataset with ID {id} not found");
        }

        await repository.DeleteAsync(dataset);

        logger.LogInformation("Dataset deleted successfully with ID {DatasetId}", id);

        return NoContent();
    }
}
