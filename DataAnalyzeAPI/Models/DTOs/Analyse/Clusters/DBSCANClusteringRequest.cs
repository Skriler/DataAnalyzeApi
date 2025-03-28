namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clusters;

public class DBSCANClusteringRequest : BaseClusteringRequest
{
    public double Epsilon { get; set; }
    public int MinPoints { get; set; }
}
