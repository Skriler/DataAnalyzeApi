using DataAnalyzeApi.Mappers.Analysis.Domain;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Requests;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;
using DataAnalyzeApi.Services.Analysis.Comparers;
using DataAnalyzeApi.Services.Analysis.Results;
using DataAnalyzeApi.Services.Cache;

namespace DataAnalyzeApi.Services.Analysis.Core;

public class SimilarityService
    : BaseAnalysisService<
        SimilarityRequest,
        SimilarityAnalysisResult,
        SimilarityAnalysisResultDto,
        SimilarityAnalysisResultService>
{
    private const string analysisType = "similarity";

    private readonly SimilarityDomainAnalysisMapper analysisMapper;
    private readonly SimilarityComparer comparer;

    public SimilarityService(
        SimilarityDomainAnalysisMapper analysisMapper,
        SimilarityComparer comparer,
        AnalysisCacheService<SimilarityAnalysisResultDto> cacheService,
        SimilarityAnalysisResultService resultService
    ) : base(cacheService, resultService, analysisType)
    {
        this.analysisMapper = analysisMapper;
        this.comparer = comparer;
    }

    /// <summary>
    /// Performs similarity analysis on the given dataset and returns the result.
    /// </summary>
    public async Task<SimilarityAnalysisResultDto> PerformAnalysisAsync(
        DatasetModel dataset,
        SimilarityRequest? request)
    {
        var similarities = comparer.CompareAllObjects(dataset);

        var similaritiesDto = analysisMapper.MapList(
            similarities,
            request?.IncludeParameters ?? false);

        var result = new SimilarityAnalysisResultDto
        {
            DatasetId = dataset.Id,
            Similarities = similaritiesDto,
        };

        var requestHash = GenerateRequestHash(request);

        await resultService.SaveDtoAsync(result, requestHash);
        await cacheService.SetAsync(analysisType, dataset.Id, requestHash, result);

        return result;
    }
}
