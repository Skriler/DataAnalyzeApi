using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Similarity;

public class SimilarityRequest
{
    public double DeviationPercent { get; set; } = 10;

    public List<ParameterSettings> ParameterSettings { get; set; } = new();
}
