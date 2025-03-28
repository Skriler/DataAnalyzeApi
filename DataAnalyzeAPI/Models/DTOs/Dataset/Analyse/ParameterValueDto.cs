namespace DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

public record ParameterValueDto(
    string Value,
    ParameterStateDto Parameter
);
