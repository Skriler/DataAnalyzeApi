using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;
using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;

public abstract class BaseClusteringRequest
{
    public List<ParameterSettingsDto> ParameterSettings { get; set; } = new();

    public NumericDistanceMetricType NumericMetric { get; set; } = NumericDistanceMetricType.Euclidean;

    public CategoricalDistanceMetricType CategoricalMetric { get; set; } = CategoricalDistanceMetricType.Hamming;
}
