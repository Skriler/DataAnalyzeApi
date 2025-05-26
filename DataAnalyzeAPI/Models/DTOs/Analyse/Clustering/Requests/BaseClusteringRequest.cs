using DataAnalyzeApi.Attributes;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings;
using DataAnalyzeApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Requests;

public abstract record BaseClusteringRequest
{
    [UniqueParameterId]
    public List<ParameterSettingsDto> ParameterSettings { get; init; } = new();

    [EnumDataType(typeof(NumericDistanceMetricType))]
    public NumericDistanceMetricType NumericMetric { get; init; } = NumericDistanceMetricType.Euclidean;

    [EnumDataType(typeof(CategoricalDistanceMetricType))]
    public CategoricalDistanceMetricType CategoricalMetric { get; init; } = CategoricalDistanceMetricType.Hamming;

    /// <summary>
    /// Include ParameterValues dictionary in responce.
    /// </summary>
    public bool IncludeParameters { get; init; }
}
