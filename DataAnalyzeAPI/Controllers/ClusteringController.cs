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
    private readonly ClusterMapper clusterMapper;
    private readonly DatasetNormalizer datasetNormalizer;
    private readonly ClustererFactory clustererFactory;

    public ClusteringController(
        DatasetRepository repository,
        DatasetSettingsMapper datasetSettingsMapper,
        ClusterMapper clusterMapper,
        DatasetNormalizer datasetNormalizer,
        ClustererFactory clustererFactory)
    {
        this.repository = repository;
        this.datasetSettingsMapper = datasetSettingsMapper;
        this.clusterMapper = clusterMapper;
        this.datasetNormalizer = datasetNormalizer;
        this.clustererFactory = clustererFactory;
    }

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
        var clusters = clusterer.Cluster(normalizedDataset, settings);
        var clustersDto = clusterMapper.MapList(clusters, settings.IncludeParameters);

        var clusteringResult = new ClusteringResult()
        {
            DatasetId = datasetId,
            Clusters = clustersDto,
        };

        return Ok(clusteringResult);
    }
}
