using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.Domain.Settings;

public class DBSCANSettings : IClusterSettings
{
    public ClusterAlgorithm Algorithm => ClusterAlgorithm.DBSCAN;

    public NumericDistanceMetricType NumericMetric { get; set; }

    public CategoricalDistanceMetricType CategoricalMetric { get; set; }

    public double Epsilon { get; set; }

    public int MinPoints { get; set; }

    public bool IncludeParameters { get; set; }
}
