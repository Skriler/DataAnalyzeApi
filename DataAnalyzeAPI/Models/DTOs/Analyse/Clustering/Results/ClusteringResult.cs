using DataAnalyzeAPI.Models.Domain.Clustering;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;

public class ClusteringResult
{
    public long DatasetId { get; set; }

    public List<Cluster> Clusters { get; set; } = new();
}
