using DataAnalyzeApi.Attributes;

namespace DataAnalyzeApi.Models.DTOs.Analysis;

public abstract record BaseAnalysisRequest
{
    [UniqueParameterId]
    public List<ParameterSettingsDto> ParameterSettings { get; init; } = [];

    /// <summary>
    /// Include ParameterValues dictionary in responce.
    /// </summary>
    public bool IncludeParameters { get; init; }
}
