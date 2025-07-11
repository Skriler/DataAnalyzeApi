using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities.Analysis.Clustering;

public class Cluster
{
    [Key]
    public long Id { get; set; }

    public long ClusterGroupId { get; set; }
    [JsonIgnore] public ClusterGroup ClusterGroup { get; set; } = default!;

    public long DataObjectId { get; set; }
    [JsonIgnore] public DataObject DataObject { get; set; } = default!;
}
