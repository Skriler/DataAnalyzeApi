using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Domain.Settings;

public class KMeansSettings : IClusterSettings
{
    public ClusterAlgorithm Algorithm => ClusterAlgorithm.KMeans;

    public NumericDistanceMetricType NumericMetric { get; set; }

    public CategoricalDistanceMetricType CategoricalMetric { get; set; }

    public int MaxIterations { get; set; }

    public int NumberOfClusters { get; set; }

    public bool IncludeParameters { get; set; }
}
