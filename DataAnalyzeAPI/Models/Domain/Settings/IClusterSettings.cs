using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.Domain.Settings;

public interface IClusterSettings
{
    ClusterAlgorithm Algorithm { get; }

    NumericDistanceMetricType NumericMetric { get; set; }

    CategoricalDistanceMetricType CategoricalMetric { get; set; }

    bool IncludeParameters { get; set; }
}
