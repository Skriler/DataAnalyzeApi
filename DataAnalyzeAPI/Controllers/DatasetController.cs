using AutoMapper;
using DataAnalyzeAPI.Models.DTOs.Create;
using DataAnalyzeAPI.Models.Entities;
using DataAnalyzeAPI.Services.DAL;
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
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var datasets = await repository.GetAllAsync();

        return Ok(datasets);
    }

    /// <summary>
    /// Get dataset by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var dataset = await repository.GetByIdAsync(id);

        if (dataset == null)
        {
            return NotFound();
        }

        var dto = mapper.Map<DatasetDto>(dataset);

        return Ok(dto);
    }

    /// <summary>
    /// Create new dataset.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DatasetDto dto)
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
