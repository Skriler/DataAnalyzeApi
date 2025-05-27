using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Domain.Settings;

public abstract record BaseClusterSettings(
    ClusterAlgorithm Algorithm,
    NumericDistanceMetricType NumericMetric,
    CategoricalDistanceMetricType CategoricalMetric,
    bool IncludeParameters
);