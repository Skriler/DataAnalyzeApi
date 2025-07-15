using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Domain.Settings;

public record KMeansSettings(
    NumericDistanceMetricType NumericMetric,
    CategoricalDistanceMetricType CategoricalMetric,
    bool IncludeParameters,
    int MaxIterations,
    int NumberOfClusters
) : BaseClusterSettings(
    ClusteringAlgorithm.KMeans,
    NumericMetric,
    CategoricalMetric,
    IncludeParameters
);
