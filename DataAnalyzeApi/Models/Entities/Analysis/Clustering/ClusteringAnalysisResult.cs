using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Entities.Analysis.Clustering;

[Index(nameof(DatasetId), Name = "IX_ClusteringAnalysisResults_DatasetId")]
[Index(nameof(RequestHash), Name = "IX_ClusteringAnalysisResults_RequestHash")]
public class ClusteringAnalysisResult : AnalysisResult
{
    public ClusteringAlgorithm Algorithm { get; set; }

    [JsonIgnore]
    public List<Cluster> Clusters { get; set; } = [];
}
