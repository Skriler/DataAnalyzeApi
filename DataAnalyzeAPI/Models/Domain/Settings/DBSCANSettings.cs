using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Models.Domain.Settings;

public class DBSCANSettings : IClusterSettings
{
    public double Epsilon { get; set; }

    public int MinPoints { get; set; }

    public ClusterAlgorithm GetAlgorithm() => ClusterAlgorithm.DBSCAN;
}
