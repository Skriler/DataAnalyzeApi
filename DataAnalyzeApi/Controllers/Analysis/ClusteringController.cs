using DataAnalyzeApi.Attributes;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Services.Analysis.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Route("api/analysis/clustering")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class ClusteringController(
    DatasetService datasetService,
    ClusteringService clusteringService,
    ILogger<ClusteringController> logger
    ) : ControllerBase
{
    protected readonly DatasetService datasetService = datasetService;
    protected readonly ILogger<ClusteringController> logger = logger;
    private readonly ClusteringService clusteringService = clusteringService;

    /// <summary>
    /// Performs K-Means clustering on the specified dataset.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">K-Means clustering configuration parameters</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    [HttpPost("kmeans/{datasetId:long}")]
    [ProducesResponseType(typeof(ClusteringAnalysisResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClusteringAnalysisResultDto>> CalculateKMeansClusters(
        [FromRoute][ValidId] long datasetId,
        [FromBody] KMeansClusterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
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
    [ProducesResponseType(typeof(ClusteringAnalysisResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClusteringAnalysisResultDto>> CalculateDBSCANClusters(
        [FromRoute][ValidId] long datasetId,
        [FromBody] DBSCANClusterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
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
    [ProducesResponseType(typeof(ClusteringAnalysisResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClusteringAnalysisResultDto>> CalculateAgglomerativeClusters(
        [FromRoute][ValidId] long datasetId,
        [FromBody] AgglomerativeClusterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
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
    private async Task<ActionResult<ClusteringAnalysisResultDto>> CalculateClusters<TSettings>(
        long datasetId,
        BaseClusteringRequest request,
        TSettings settings
        ) where TSettings : BaseClusterSettings
    {
        var storedResult = await clusteringService.GetResultFromCacheOrDbAsync(
            datasetId,
            request);

        if (storedResult != null)
        {
            logger.LogInformation(
                "Returning stored {Algorithm} clustering result for dataset {DatasetId}",
                settings.Algorithm,
                datasetId);

            return storedResult;
        }

        var dataset = await datasetService.GetPreparedNormalizedDatasetAsync(
            datasetId,
            request.ParameterSettings);

        var result = await clusteringService.PerformAnalysisAsync(
            dataset,
            request,
            settings);

        logger.LogInformation(
            "Successfully calculated {Algorithm} clustering for dataset {DatasetId} with {ClusterCount} clusters",
            settings.Algorithm,
            datasetId,
            result.Clusters.Count);

        return result;
    }
}
