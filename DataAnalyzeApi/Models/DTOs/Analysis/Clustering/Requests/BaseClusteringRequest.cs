using System.ComponentModel.DataAnnotations;
using DataAnalyzeApi.Attributes;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;

public abstract record BaseClusteringRequest
{
    [UniqueParameterId]
    public List<ParameterSettingsDto> ParameterSettings { get; init; } = [];

    [EnumDataType(typeof(NumericDistanceMetricType))]
    public NumericDistanceMetricType NumericMetric { get; init; } = NumericDistanceMetricType.Euclidean;

    [EnumDataType(typeof(CategoricalDistanceMetricType))]
    public CategoricalDistanceMetricType CategoricalMetric { get; init; } = CategoricalDistanceMetricType.Hamming;

    /// <summary>
    /// Include ParameterValues dictionary in responce.
    /// </summary>
    public bool IncludeParameters { get; init; }
}
