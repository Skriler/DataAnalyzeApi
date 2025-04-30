using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeApi.Services.Analyse.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Route("api/analyse/clustering")]
[Authorize(Policy = "UserOrAdmin")]
public class ClusteringController : ControllerBase
{
    private readonly DatasetService datasetService;
    private readonly ClusteringService clusteringService;

    public ClusteringController(
        DatasetService datasetService,
        ClusteringService clusteringService)
    {
        this.datasetService = datasetService;
        this.clusteringService = clusteringService;
    }

    /// <summary>
    /// Performs K-Means clustering on the specified dataset.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">K-Means clustering configuration parameters</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    [HttpPost("kmeans/{datasetId}")]
    public async Task<IActionResult> CalculateKMeansClusters(
        long datasetId,
        [FromBody] KMeansClusteringRequest request)
    {
        var settings = new KMeansSettings
        {
            NumericMetric = request.NumericMetric,
            CategoricalMetric = request.CategoricalMetric,
            IncludeParameters = request.IncludeParameters,
            MaxIterations = request.MaxIterations,
            NumberOfClusters = request.NumberOfClusters,
        };

        return await CalculateClusters(datasetId, request, settings);
    }

    /// <summary>
    /// Performs DBSCAN (Density-Based Spatial Clustering of Applications with Noise)
    /// clustering on the specified dataset.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">DBSCAN clustering configuration parameters</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    [HttpPost("dbscan/{datasetId}")]
    public async Task<IActionResult> CalculateDBSCANClusters(
        long datasetId,
        [FromBody] DBSCANClusteringRequest request)
    {
        var settings = new DBSCANSettings
        {
            NumericMetric = request.NumericMetric,
            CategoricalMetric = request.CategoricalMetric,
            IncludeParameters = request.IncludeParameters,
            Epsilon = request.Epsilon,
            MinPoints = request.MinPoints,
        };

        return await CalculateClusters(datasetId, request, settings);
    }

    /// <summary>
    /// Performs Agglomerative Hierarchical clustering on the specified dataset.
    /// </summary>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">Agglomerative clustering configuration parameters</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    [HttpPost("agglomerative/{datasetId}")]
    public async Task<IActionResult> CalculateAgglomerativeClusters(
        long datasetId,
        [FromBody] AgglomerativeClusteringRequest request)
    {
        var settings = new AgglomerativeSettings
        {
            NumericMetric = request.NumericMetric,
            CategoricalMetric = request.CategoricalMetric,
            IncludeParameters = request.IncludeParameters,
            Threshold = request.Threshold,
        };

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
        TSettings settings) where TSettings : IClusterSettings
    {
        var cachedResult = await clusteringService.GetCachedResultAsync(
            datasetId,
            settings.Algorithm,
            request);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var dataset = await datasetService.GetPreparedNormalizedDatasetAsync(
            datasetId,
            request.ParameterSettings);

        var clusteringResult = await clusteringService.PerformAnalysisAsync(
            dataset,
            request,
            settings);

        return Ok(clusteringResult);
    }
}
