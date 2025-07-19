using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.Models.Entities.Analysis.Clustering;

[Index(nameof(ClusteringAnalysisResultId), Name = "IX_DataObjectCoordinates_ClusteringAnalysisResultId")]
public class DataObjectCoordinate
{
    [Key]
    public long Id { get; set; }

    public double X { get; set; }
    public double Y { get; set; }

    public long ObjectId { get; set; }
    [JsonIgnore] public DataObject Object { get; set; } = default!;

    [JsonIgnore] public long ClusteringAnalysisResultId { get; set; }
    [JsonIgnore] public ClusteringAnalysisResult ClusteringAnalysisResult { get; set; } = default!;
}
