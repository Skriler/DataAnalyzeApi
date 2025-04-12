using DataAnalyzeAPI.Models.DTOs.Dataset;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;

public record ClusterDto(
    string Name,
    List<DataObjectDto> Objects
);
