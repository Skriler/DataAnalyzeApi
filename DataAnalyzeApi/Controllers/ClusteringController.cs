using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeApi.Services.Analyse.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Route("api/analyse/clustering")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class ClusteringController(
    DatasetService datasetService,
    ClusteringService clusteringService,
    ILogger<ClusteringController> logger
    ) : ControllerBase
{
    private readonly DatasetService datasetService = datasetService;
    private readonly ClusteringService clusteringService = clusteringService;
    private readonly ILogger<ClusteringController> logger = logger;

    /// <summary>
    /// Performs K-Means clustering on the specified dataset.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">K-Means clustering configuration parameters</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    [HttpPost("kmeans/{datasetId:long}")]
    [ProducesResponseType(typeof(ClusteringResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CalculateKMeansClusters(
        long datasetId,
        [FromBody] KMeansClusteringRequest request)
    {
        var validationResult = ValidateRequest(datasetId);

        if (validationResult != null)
        {
            return validationResult;
        }

        var settings = new KMeansSettings(
            request.NumericMetric,
            request.CategoricalMetric,
            request.IncludeParameters,
            request.MaxIterations,
            request.NumberOfClusters);

        return await CalculateClusters(datasetId, request, settings);
    }

    /// <summary>
    /// Performs DBSCAN (Density-Based Spatial Clustering of Applications with Noise)
    /// clustering on the specified dataset.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">DBSCAN clustering configuration parameters</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    [HttpPost("dbscan/{datasetId:long}")]
    [ProducesResponseType(typeof(ClusteringResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CalculateDBSCANClusters(
        long datasetId,
        [FromBody] DBSCANClusteringRequest request)
    {
        var validationResult = ValidateRequest(datasetId);

        if (validationResult != null)
        {
            return validationResult;
        }

        var settings = new DBSCANSettings(
            request.NumericMetric,
            request.CategoricalMetric,
            request.IncludeParameters,
            request.Epsilon,
            request.MinPoints);

        return await CalculateClusters(datasetId, request, settings);
    }

    /// <summary>
    /// Performs Agglomerative Hierarchical clustering on the specified dataset.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">Agglomerative clustering configuration parameters</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    [HttpPost("agglomerative/{datasetId:long}")]
    [ProducesResponseType(typeof(ClusteringResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CalculateAgglomerativeClusters(
        long datasetId,
        [FromBody] AgglomerativeClusteringRequest request)
    {
        var validationResult = ValidateRequest(datasetId);

        if (validationResult != null)
        {
            return validationResult;
        }

        var settings = new AgglomerativeSettings(
            request.NumericMetric,
            request.CategoricalMetric,
            request.IncludeParameters,
            request.Threshold);

        return await CalculateClusters(datasetId, request, settings);
    }

    /// <summary>
    /// Calculates clusters for the specified dataset using the given settings and request data.
    /// Returns a cached result if available; otherwise, performs a new clustering analysis.
    /// </summary>
    /// <typeparam name="TSettings">The type of clustering settings</typeparam>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">The base clustering request containing configuration parameters</param>
    /// <param name="settings">The specific settings for the selected algorithm</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    private async Task<IActionResult> CalculateClusters<TSettings>(
        long datasetId,
        BaseClusteringRequest request,
        TSettings settings
        ) where TSettings : BaseClusterSettings
    {
        var cachedResult = await clusteringService.GetCachedResultAsync(
            datasetId,
            settings.Algorithm,
            request);

        if (cachedResult != null)
        {
            logger.LogInformation(
                "Returning cached {Algorithm} clustering result for dataset {DatasetId}",
                datasetId,
                settings.Algorithm);
            return Ok(cachedResult);
        }

        var dataset = await datasetService.GetPreparedNormalizedDatasetAsync(
            datasetId,
            request.ParameterSettings);

        var clusteringResult = await clusteringService.PerformAnalysisAsync(
            dataset,
            request,
            settings);

        logger.LogInformation(
            "Successfully calculated {Algorithm} clustering for dataset {DatasetId} with {ClusterCount} clusters",
            settings.Algorithm,
            datasetId,
            clusteringResult.Clusters.Count);

        return Ok(clusteringResult);
    }

    /// <summary>
    /// Validates the dataset ID and the model state.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to validate</param>
    /// <returns>
    /// An action result representing a validation error,
    /// or null if validation succeeds.
    /// </returns>
    private IActionResult? ValidateRequest(long datasetId)
    {
        if (datasetId <= 0)
        {
            return BadRequest("Invalid dataset ID");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return null;
    }
}
