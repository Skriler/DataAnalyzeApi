using DataAnalyzeApi.Mappers.Analysis;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Factories.Clusterer;
using DataAnalyzeApi.Services.Cache;

namespace DataAnalyzeApi.Services.Analysis.Core;

public class ClusteringService(
    DatasetService datasetService,
    AnalysisMapper analysisMapper,
    IClustererFactory clustererFactory,
    ClusteringCacheService cacheService
    ) : BaseAnalysisService(datasetService, analysisMapper)
{
    private readonly IClustererFactory clustererFactory = clustererFactory;
    private readonly ClusteringCacheService cacheService = cacheService;

    /// <summary>
    /// Performs clustering analysis on the given dataset using the specified algorithm and settings.
    /// </summary>
    public async Task<ClusterAnalysisResultDto> PerformAnalysisAsync<TSettings>(
        DatasetModel dataset,
        BaseClusteringRequest request,
        TSettings settings) where TSettings : BaseClusterSettings
    {
        var clusterer = clustererFactory.Get<TSettings>(settings.Algorithm);
        var clusters = clusterer.Cluster(dataset.Objects, settings);

        var clustersDto = analysisMapper.MapClusterList(clusters, settings.IncludeParameters);

        var clusteringResult = new ClusterAnalysisResultDto()
        {
            DatasetId = dataset.Id,
            Clusters = clustersDto,
        };

        await cacheService.CacheResultAsync(dataset.Id, settings.Algorithm, request, clusteringResult);

        return clusteringResult;
    }

    /// <summary>
    /// Retrieves a cached clustering result for the given dataset, algorithm, and request, if available.
    /// </summary>
    public async Task<ClusterAnalysisResultDto?> GetCachedResultAsync(
        long datasetId,
        ClusterAlgorithm algorithm,
        BaseClusteringRequest request)
    {
        return await cacheService.GetCachedResultAsync(datasetId, algorithm, request);
    }
}
