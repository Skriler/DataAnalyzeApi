using System.ComponentModel.DataAnnotations;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Requests;

public abstract record BaseClusteringRequest : BaseAnalysisRequest
{
    [EnumDataType(typeof(NumericDistanceMetricType))]
    public NumericDistanceMetricType NumericMetric { get; init; } = NumericDistanceMetricType.Euclidean;

    [EnumDataType(typeof(CategoricalDistanceMetricType))]
    public CategoricalDistanceMetricType CategoricalMetric { get; init; } = CategoricalDistanceMetricType.Hamming;
}
