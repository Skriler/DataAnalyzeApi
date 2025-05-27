using DataAnalyzeApi.Models.DTOs.Dataset;

namespace DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Results;

public record ClusterDto(
    string Name,
    List<DataObjectDto> Objects
);
