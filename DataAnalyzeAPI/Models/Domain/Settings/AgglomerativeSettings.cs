using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.Domain.Settings;

public class AgglomerativeSettings : IClusterSettings
{
    public NumericDistanceMetricType NumericMetric { get; set; }

    public CategoricalDistanceMetricType CategoricalMetric { get; set; }

    public double Threshold { get; set; }

    public bool IncludeParameters { get; set; }

    public ClusterAlgorithm GetAlgorithm() => ClusterAlgorithm.HierarchicalAgglomerative;
}
