namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;

public record ClusterDto(
    string Name,
    List<DataObjectAnalysisDto> Objects
);
