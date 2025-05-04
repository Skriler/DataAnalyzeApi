using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Domain.Settings;

public record AgglomerativeSettings(
    NumericDistanceMetricType NumericMetric,
    CategoricalDistanceMetricType CategoricalMetric,
    bool IncludeParameters,
    double Threshold
) : BaseClusterSettings(
    ClusterAlgorithm.Agglomerative,
    NumericMetric,
    CategoricalMetric,
    IncludeParameters
);
