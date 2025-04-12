using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.Domain.Settings;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeAPI.Models.Enums;
using DataAnalyzeAPI.Services.Analyse.Clusterers;
using DataAnalyzeAPI.Services.DAL;
using DataAnalyzeAPI.Services.Normalizers;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/analyse/clustering")]
public class ClusteringController : Controller
{
    private readonly DatasetRepository repository;
    private readonly DatasetSettingsMapper datasetSettingsMapper;
    private readonly AnalysisMapper analysisMapper;
    private readonly DatasetNormalizer datasetNormalizer;
    private readonly ClustererFactory clustererFactory;

    public ClusteringController(
        DatasetRepository repository,
        DatasetSettingsMapper datasetSettingsMapper,
        AnalysisMapper analysisMapper,
        DatasetNormalizer datasetNormalizer,
        ClustererFactory clustererFactory)
    {
        this.repository = repository;
        this.datasetSettingsMapper = datasetSettingsMapper;
        this.analysisMapper = analysisMapper;
        this.datasetNormalizer = datasetNormalizer;
        this.clustererFactory = clustererFactory;
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
        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
        {
            return NotFound($"Dataset with ID {datasetId} not found.");
        }

        var mappedDataset = datasetSettingsMapper.Map(dataset, request.ParameterSettings);
        var normalizedDataset = datasetNormalizer.Normalize(mappedDataset);

        var clusterer = clustererFactory.Get<TSettings>(algorithm);
        var clusters = clusterer.Cluster(normalizedDataset.Objects, settings);
        var clustersDto = analysisMapper.MapClusterList(clusters, settings.IncludeParameters);

        var clusteringResult = new ClusteringResult()
        {
            DatasetId = datasetId,
            Clusters = clustersDto,
        };

        return Ok(clusteringResult);
    }
}
