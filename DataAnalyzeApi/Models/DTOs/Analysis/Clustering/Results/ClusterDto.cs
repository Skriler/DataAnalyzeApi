namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;

public record ClusterDto
{
    public string Name { get; set; } = string.Empty;

    public List<DataObjectClusteringAnalysisDto> Objects { get; set; } = [];
}
