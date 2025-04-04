namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;

public class KMeansClusteringRequest : BaseClusteringRequest
{
    public int MaxIterations { get; set; }

    public int NumberOfClusters { get; set; }
}
