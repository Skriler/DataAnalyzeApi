using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Entities.Analysis.Clustering;

[Index(nameof(DatasetId), Name = "IX_ClusterAnalysisResults_DatasetId")]
[Index(nameof(RequestHash), Name = "IX_ClusterAnalysisResults_RequestHash")]
public class ClusterAnalysisResult : AnalysisResult
{
    public ClusterAlgorithm Algorithm { get; set; }

    [JsonIgnore]
    public List<Cluster> Clusters { get; set; } = [];
}
