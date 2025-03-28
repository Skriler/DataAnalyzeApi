namespace DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

public record DataObjectDto(
    long Id,
    string Name,
    List<ParameterValueDto> Values
);
