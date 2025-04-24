using DataAnalyzeAPI.Models.Domain.Settings;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeAPI.Models.Enums;
using DataAnalyzeAPI.Services.Analyse.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeAPI.Controllers;

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

        return await CalculateClusters(
            datasetId,
            request,
            ClusterAlgorithm.KMeans,
            settings);
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

        return await CalculateClusters(
            datasetId,
            request,
            ClusterAlgorithm.DBSCAN,
            settings);
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

        return await CalculateClusters(
            datasetId,
            request,
            ClusterAlgorithm.HierarchicalAgglomerative,
            settings);
    }

    /// <summary>
    /// Generic method that performs clustering analysis on a dataset using the specified algorithm and settings.
    /// This private method handles the common workflow for all clustering algorithms:
    /// 1. Retrieves the dataset
    /// 2. Maps and normalizes the dataset
    /// 3. Performs clustering using the appropriate algorithm
    /// 4. Maps the results to DTOs for response
    /// </summary>
    /// <typeparam name="TSettings">The type of clustering settings</typeparam>
    /// <param name="datasetId">The ID of the dataset to analyze</param>
    /// <param name="request">The base clustering request containing configuration parameters</param>
    /// <param name="algorithm">The clustering algorithm to use</param>
    /// <param name="settings">The specific settings for the selected algorithm</param>
    /// <returns>An action result containing the clustering results or an error response</returns>
    private async Task<IActionResult> CalculateClusters<TSettings>(
        long datasetId,
        BaseClusteringRequest request,
        ClusterAlgorithm algorithm,
        TSettings settings) where TSettings : IClusterSettings
    {
        var cachedResult = await clusteringService.GetCachedResultAsync(datasetId, algorithm, request);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var mappedDataset = await datasetService.GetPreparedDatasetAsync(datasetId, request.ParameterSettings);
        var normalizedDataset = datasetService.NormalizeDataset(mappedDataset);

        var clusteringResult = await clusteringService.CalculateClustersAsync(normalizedDataset, request, algorithm, settings);

        return Ok(clusteringResult);
    }
}
