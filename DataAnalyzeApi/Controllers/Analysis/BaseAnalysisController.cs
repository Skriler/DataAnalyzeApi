using DataAnalyzeApi.Services.Analysis.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public abstract class BaseAnalysisController<T>(
    DatasetService datasetService,
    ILogger<T> logger
    ) : ControllerBase
    where T : class
{
    protected readonly DatasetService datasetService = datasetService;
    protected readonly ILogger<T> logger = logger;
}
