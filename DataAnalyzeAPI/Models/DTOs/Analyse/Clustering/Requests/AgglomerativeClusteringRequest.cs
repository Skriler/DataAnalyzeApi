namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Requests;

public class AgglomerativeClusteringRequest : BaseClusteringRequest
{
    public double Threshold { get; set; }
}
