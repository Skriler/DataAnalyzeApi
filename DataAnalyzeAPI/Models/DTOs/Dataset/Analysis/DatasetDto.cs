namespace DataAnalyzeAPI.Models.DTOs.Dataset.Analysis;

public record DatasetDto(
    long Id,
    string Name,
    List<ParameterStateDto> Parameters,
    List<DataObjectDto> Objects
);
