using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Domain.Settings;

public record DBSCANSettings(
    NumericDistanceMetricType NumericMetric,
    CategoricalDistanceMetricType CategoricalMetric,
    bool IncludeParameters,
    double Epsilon,
    int MinPoints
) : BaseClusterSettings(
    ClusteringAlgorithm.DBSCAN,
    NumericMetric,
    CategoricalMetric,
    IncludeParameters
);