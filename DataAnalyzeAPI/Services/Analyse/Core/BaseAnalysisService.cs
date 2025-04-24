using DataAnalyzeAPI.Mappers;

namespace DataAnalyzeAPI.Services.Analyse.Core;

public abstract class BaseAnalysisService
{
    protected readonly DatasetService datasetService;
    protected readonly AnalysisMapper analysisMapper;

    protected BaseAnalysisService(
        DatasetService datasetService,
        AnalysisMapper analysisMapper)
    {
        this.datasetService = datasetService;
        this.analysisMapper = analysisMapper;
    }
}
