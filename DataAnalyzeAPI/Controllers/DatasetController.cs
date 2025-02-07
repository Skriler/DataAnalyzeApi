using DataAnalyzeAPI.Models.DTOs;
using DataAnalyzeAPI.Services.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeAPI.Controllers;

[Route("api/datasets")]
[ApiController]
public class DatasetController : ControllerBase
{
    

    public DatasetController(DataAnalyzeDbContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DatasetCreateDto dto)
    {
        if (dto == null || dto.Objects.Count == 0 || dto.Parameters.Count == 0)
        {
            return BadRequest("Invalid dataset data.");
        }

        
    }
}
