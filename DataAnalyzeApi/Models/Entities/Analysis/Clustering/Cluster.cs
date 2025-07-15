using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities.Analysis.Clustering;

public class Cluster
{
    [Key]
    public long Id { get; set; }

    [JsonIgnore]
    public List<DataObject> Objects { get; set; } = [];

    public long ClusteringAnalysisResultId { get; set; }
    [JsonIgnore] public ClusteringAnalysisResult ClusteringAnalysisResult { get; set; } = default!;
}
