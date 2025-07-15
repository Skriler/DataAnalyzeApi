using DataAnalyzeApi.Mappers.Analysis;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Services.Analysis.Factories.Clusterer;
using DataAnalyzeApi.Services.Analysis.Results;
using DataAnalyzeApi.Services.Cache;

namespace DataAnalyzeApi.Services.Analysis.Core;

public class ClusteringService
    : BaseAnalysisService<
        BaseClusteringRequest,
        ClusteringAnalysisResult,
        ClusteringAnalysisResultDto,
        ClusteringAnalysisResultService>
{
    private const string analysisType = "clustering";

    private readonly IClustererFactory clustererFactory;

    public ClusteringService(
        ModelAnalysisMapper modelAnalysisMapper,
        IClustererFactory clustererFactory,
        AnalysisCacheService<ClusteringAnalysisResultDto> cacheService,
        ClusteringAnalysisResultService resultService
    ) : base(modelAnalysisMapper, cacheService, resultService, analysisType)
    {
        this.clustererFactory = clustererFactory;
    }

    /// <summary>
    /// Performs clustering analysis on the given dataset using the specified algorithm and settings.
    /// </summary>
    public async Task<ClusteringAnalysisResultDto> PerformAnalysisAsync<TSettings>(
        DatasetModel dataset,
        BaseClusteringRequest request,
        TSettings settings) where TSettings : BaseClusterSettings
    {
        var clusterer = clustererFactory.Get<TSettings>(settings.Algorithm);
        var clusters = clusterer.Cluster(dataset.Objects, settings);

        var clustersDto = modelAnalysisMapper.MapClusterList(
            clusters,
            settings.IncludeParameters);

        var result = new ClusteringAnalysisResultDto()
        {
            DatasetId = dataset.Id,
            Clusters = clustersDto,
        };

        var requestHash = GenerateRequestHash(request);

        await resultService.SaveDtoAsync(result, requestHash, settings.Algorithm);
        await cacheService.SetAsync(analysisType, dataset.Id, requestHash, result);

        return result;
    }
}
