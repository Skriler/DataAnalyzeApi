using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;

public record ClusteringAnalysisResultDto : BaseAnalysisResultDto
{
    public ClusteringAlgorithm Algorithm { get; set; }

    public List<ClusterDto> Clusters { get; set; } = [];
}
