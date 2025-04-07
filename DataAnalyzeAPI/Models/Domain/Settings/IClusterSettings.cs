using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.Domain.Settings;

public interface IClusterSettings
{
    NumericDistanceMetricType NumericMetric { get; set; }

    CategoricalDistanceMetricType CategoricalMetric { get; set; }

    public ClusterAlgorithm GetAlgorithm();
}
