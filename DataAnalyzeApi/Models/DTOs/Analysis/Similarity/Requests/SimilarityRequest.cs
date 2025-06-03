using DataAnalyzeApi.Attributes;

namespace DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Requests;

public record SimilarityRequest
{
    [UniqueParameterId]
    public List<ParameterSettingsDto> ParameterSettings { get; init; } = new();

    /// <summary>
    /// Include ParameterValues dictionary in responce.
    /// </summary>
    public bool IncludeParameters { get; init; }
}
