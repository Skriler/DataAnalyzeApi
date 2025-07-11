using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities.Analysis.Clustering;

public class ClusterGroup
{
    [Key]
    public long Id { get; set; }

    public long ClusteringAnalysisId { get; set; }
    [JsonIgnore] public ClusteringAnalysis ClusteringAnalysis { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public int ClusterIndex { get; set; }

    public List<Cluster> Objects { get; set; } = [];
}
