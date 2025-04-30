using DataAnalyzeApi.Models.DTOs.Analyse.Settings;
using DataAnalyzeApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Requests;

public abstract record BaseClusteringRequest
{
    public List<ParameterSettingsDto> ParameterSettings { get; set; } = new();

    [EnumDataType(typeof(NumericDistanceMetricType))]
    public NumericDistanceMetricType NumericMetric { get; set; } = NumericDistanceMetricType.Euclidean;

    [EnumDataType(typeof(CategoricalDistanceMetricType))]
    public CategoricalDistanceMetricType CategoricalMetric { get; set; } = CategoricalDistanceMetricType.Hamming;

    /// <summary>
    /// Include ParameterValues dictionary in responce.
    /// </summary>
    public bool IncludeParameters { get; set; }
}
