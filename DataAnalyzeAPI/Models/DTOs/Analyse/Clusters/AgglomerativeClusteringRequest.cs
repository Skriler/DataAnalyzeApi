namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clusters;

public class AgglomerativeClusteringRequest : BaseClusteringRequest
{
    public double Threshold { get; set; }
}
