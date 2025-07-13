namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;

public record ClusterAnalysisResultDto
{
    public long DatasetId { get; set; }

    public List<ClusterDto> Clusters { get; set; } = [];
}
