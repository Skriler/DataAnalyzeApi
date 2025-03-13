using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Similarity;

public class SimilarityRequest
{
    [Range(0, 100)]
    public double MinSimilarityPercentage { get; set; } = 55;

    [Range(0, 100)]
    public double DeviationPercent { get; set; } = 10;

    public List<ParameterSettings> ParameterSettings { get; set; } = new();
}
