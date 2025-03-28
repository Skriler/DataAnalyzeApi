namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clusters;

public class KMeansClusteringRequest : BaseClusteringRequest
{
    public int MaxIterations { get; set; }
    public int NumberOfClusters { get; set; }
}
