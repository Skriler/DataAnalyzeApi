using DataAnalyzeAPI.Models.DTOs.Dataset;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;

public record ClusterDto(
    int Id,
    List<DataObjectDto> Objects
);
