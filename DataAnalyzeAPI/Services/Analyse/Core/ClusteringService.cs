using DataAnalyzeAPI.Mappers;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Settings;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeAPI.Models.Enums;
using DataAnalyzeAPI.Services.Analyse.Clusterers;

namespace DataAnalyzeAPI.Services.Analyse.Core;

public class ClusteringService : BaseAnalysisService
{
    private readonly ClustererFactory clustererFactory;
    private readonly ClusteringCacheService cacheService;

    public ClusteringService(
        DatasetService datasetService,
        AnalysisMapper analysisMapper,
        ClustererFactory clustererFactory,
        ClusteringCacheService cacheService)
        : base(datasetService, analysisMapper)
    {
        this.clustererFactory = clustererFactory;
        this.cacheService = cacheService;
    }

    public async Task<ClusteringResult> CalculateClustersAsync<TSettings>(
        DatasetModel dataset,
        BaseClusteringRequest request,
        ClusterAlgorithm algorithm,
        TSettings settings) where TSettings : IClusterSettings
    {
        var clusterer = clustererFactory.Get<TSettings>(algorithm);
        var clusters = clusterer.Cluster(dataset.Objects, settings);

        var clustersDto = analysisMapper.MapClusterList(clusters, settings.IncludeParameters);

        var clusteringResult = new ClusteringResult()
        {
            DatasetId = dataset.Id,
            Clusters = clustersDto,
        };

        await cacheService.CacheResultAsync(dataset.Id, algorithm, request, clusteringResult);

        return clusteringResult;
    }

    public async Task<ClusteringResult?> GetCachedResultAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        BaseClusteringRequest request)
    {
        return await cacheService.GetCachedResultAsync(datasetId, algorithm, request);
    }
}
