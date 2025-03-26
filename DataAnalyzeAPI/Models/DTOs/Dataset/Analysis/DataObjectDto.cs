namespace DataAnalyzeAPI.Models.DTOs.Dataset.Analysis;

public record DataObjectDto(
    long Id,
    string Name,
    List<ParameterValueDto> Values
);
