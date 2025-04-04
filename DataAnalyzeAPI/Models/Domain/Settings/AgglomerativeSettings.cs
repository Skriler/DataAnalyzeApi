using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.Domain.Settings;

public class AgglomerativeSettings : IClusterSettings
{
    public double Threshold { get; set; }

    public ClusterAlgorithm GetAlgorithm() => ClusterAlgorithm.HierarchicalAgglomerative;
}
