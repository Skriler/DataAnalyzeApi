using AutoMapper;
using DataAnalyzeAPI.DAL.Repositories;
using DataAnalyzeAPI.Models.DTOs.Dataset.Create;
using DataAnalyzeAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeAPI.Controllers;

[Route("api/datasets")]
[ApiController]
public class DatasetController : ControllerBase
{
    private readonly DatasetRepository repository;
    private readonly IMapper mapper;

    public DatasetController(DatasetRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    /// <summary>
    /// Get all datasets.
    /// </summary>
    /// <returns>An action result containing the list of datasets or an error response</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var datasets = await repository.GetAllAsync();

        return Ok(datasets);
    }

    /// <summary>
    /// Get dataset by id.
    /// </summary>
    /// <param name="id">The ID of the dataset to retrieve</param>
    /// <returns>An action result containing the dataset DTO or a NotFound response</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var dataset = await repository.GetByIdAsync(id);

        if (dataset == null)
        {
            return NotFound();
        }

        var dto = mapper.Map<DatasetCreateDto>(dataset);

        return Ok(dto);
    }

    /// <summary>
    /// Create new dataset.
    /// </summary>
    /// <param name="dto">The dataset creation details (DatasetCreateDto)</param>
    /// <returns>An action result indicating the outcome of the creation process</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DatasetCreateDto dto)
    {
        if (dto == null || dto.Objects.Count == 0 || dto.Parameters.Count == 0)
        {
            return BadRequest("Invalid dataset data.");
        }

        var dataset = mapper.Map<Dataset>(dto);
        await repository.AddAsync(dataset);

        return CreatedAtAction(nameof(GetById), new { id = dataset.Id }, dataset);
    }

    /// <summary>
    /// Delete dataset by id.
    /// </summary>
    /// <param name="id">The ID of the dataset to delete</param>
    /// <returns>An action result indicating the outcome of the deletion process</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var dataset = await repository.GetByIdAsync(id);

        if (dataset == null)
        {
            return NotFound();
        }

        await repository.DeleteAsync(dataset);

        return NoContent();
    }
}
