namespace DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

public record DatasetDto(
    long Id,
    string Name,
    List<ParameterStateDto> Parameters,
    List<DataObjectDto> Objects
);
