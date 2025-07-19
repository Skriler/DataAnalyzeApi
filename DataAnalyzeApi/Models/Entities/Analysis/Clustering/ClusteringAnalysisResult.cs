using System.Text.Json.Serialization;
using DataAnalyzeApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.Models.Entities.Analysis.Clustering;

[Index(nameof(DatasetId), Name = "IX_ClusteringAnalysisResults_DatasetId")]
[Index(nameof(RequestHash), Name = "IX_ClusteringAnalysisResults_RequestHash")]
public class ClusteringAnalysisResult : AnalysisResult
{
    public ClusteringAlgorithm Algorithm { get; set; }

    public List<Cluster> Clusters { get; set; } = [];

    [JsonIgnore]
    public List<DataObjectCoordinate> ObjectCoordinates { get; set; } = [];
}
