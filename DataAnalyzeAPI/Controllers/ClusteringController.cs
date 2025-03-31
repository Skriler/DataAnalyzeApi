using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clusters;
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
    private readonly DatasetNormalizer datasetNormalizer;
    private readonly ClustererFactory clustererFactory;

    public ClusteringController(
        DatasetRepository repository,
        DatasetSettingsMapper datasetSettingsMapper,
        DatasetNormalizer datasetNormalizer,
        ClustererFactory clustererFactory)
    {
        this.repository = repository;
        this.datasetSettingsMapper = datasetSettingsMapper;
        this.datasetNormalizer = datasetNormalizer;
        this.clustererFactory = clustererFactory;
    }

    [HttpPost("kmeans/{datasetId}")]
    public async Task<IActionResult> CalculateKMeansClusters(
        long datasetId,
        [FromBody] KMeansClusteringRequest request)
    {
        return await CalculateClusters(
            datasetId,
            request,
            ClusterAlgorithm.KMeans);
    }

    [HttpPost("dbscan/{datasetId}")]
    public async Task<IActionResult> CalculateDBSCANClusters(
        long datasetId,
        [FromBody] DBSCANClusteringRequest request)
    {
        return await CalculateClusters(
            datasetId,
            request,
            ClusterAlgorithm.DBSCAN);
    }

    [HttpPost("agglomerative/{datasetId}")]
    public async Task<IActionResult> CalculateAgglomerativeClusters(
        long datasetId,
        [FromBody] AgglomerativeClusteringRequest request)
    {
        return await CalculateClusters(
            datasetId,
            request,
            ClusterAlgorithm.HierarchicalAgglomerative);
    }

    private async Task<IActionResult> CalculateClusters(
        long datasetId,
        BaseClusteringRequest request,
        ClusterAlgorithm algorithm)
    {
        var dataset = await repository.GetByIdAsync(datasetId);

        if (dataset == null)
        {
            return NotFound($"Dataset with ID {datasetId} not found.");
        }

        var mappedDataset = datasetSettingsMapper.MapObjects(dataset, request.ParameterSettings);
        var normalizedDataset = datasetNormalizer.Normalize(mappedDataset);

        var clusterer = clustererFactory.Get(algorithm);
        var clusters = clusterer.Cluster(normalizedDataset);

        var clusteringResult = new ClusteringResult()
        {
            DatasetId = datasetId,
            Clusters = clusters,
        };

        return Ok(clusteringResult);
    }
}
