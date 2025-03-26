namespace DataAnalyzeAPI.Models.DTOs.Dataset.Analysis;

public record ParameterValueDto(
    string Value,
    ParameterStateDto Parameter
);
