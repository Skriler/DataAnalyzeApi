using DataAnalyzeApi.Services.Analyse.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers.Analyse;

[ApiController]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public abstract class BaseAnalyseController<T>(
    DatasetService datasetService,
    ILogger<T> logger
    ) : ControllerBase
    where T : class
{
    protected readonly DatasetService datasetService = datasetService;
    protected readonly ILogger<T> logger = logger;

    /// <summary>
    /// Validates the dataset ID and the model state,
    /// and returns appropriate error response if validation fails.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to validate</param>
    /// <param name="errorResult"> The error result if validation fails</param>
    /// <returns>True if model is valid, false otherwise</returns>
    protected bool TryValidateRequest(long datasetId, out ActionResult? errorResult)
    {
        if (datasetId <= 0)
        {
            errorResult = BadRequest("Invalid dataset ID");
            return false;
        }

        if (!ModelState.IsValid)
        {
            errorResult = BadRequest(ModelState);
            return false;
        }

        errorResult = null;
        return true;
    }
}
