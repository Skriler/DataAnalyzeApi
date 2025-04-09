namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;

public class ClusteringResult
{
    public long DatasetId { get; set; }

    public List<ClusterDto> Clusters { get; set; } = new();
}
