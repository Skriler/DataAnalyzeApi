using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Domain.Settings;

public class AgglomerativeSettings : IClusterSettings
{
    public ClusterAlgorithm Algorithm => ClusterAlgorithm.HierarchicalAgglomerative;

    public NumericDistanceMetricType NumericMetric { get; set; }

    public CategoricalDistanceMetricType CategoricalMetric { get; set; }

    public double Threshold { get; set; }

    public bool IncludeParameters { get; set; }
}
