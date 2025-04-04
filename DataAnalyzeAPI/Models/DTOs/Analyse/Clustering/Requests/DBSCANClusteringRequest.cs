namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;

public class DBSCANClusteringRequest : BaseClusteringRequest
{
    public double Epsilon { get; set; }

    public int MinPoints { get; set; }
}
