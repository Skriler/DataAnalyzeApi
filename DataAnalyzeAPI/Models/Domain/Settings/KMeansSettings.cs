using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.Domain.Settings;

public class KMeansSettings : IClusterSettings
{
    public int MaxIterations { get; set; }

    public int NumberOfClusters { get; set; }

    public ClusterAlgorithm GetAlgorithm() => ClusterAlgorithm.KMeans;
}
