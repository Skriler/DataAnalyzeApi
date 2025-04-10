using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Requests;

public class SimilarityRequest
{
    public List<ParameterSettingsDto> ParameterSettings { get; set; } = new();

    /// <summary>
    /// Include ParameterValues dictionary in responce.
    /// </summary>
    public bool IncludeParameters { get; set; }
}
