using DataAnalyzeApi.Mappers.Analysis;

namespace DataAnalyzeApi.Services.Analysis.Core;

/// <summary>
/// Base class for analysis services.
/// </summary>
public abstract class BaseAnalysisService(
    DatasetService datasetService,
    AnalysisMapper analysisMapper)
{
    protected readonly DatasetService datasetService = datasetService;
    protected readonly AnalysisMapper analysisMapper = analysisMapper;
}
