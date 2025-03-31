using DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.DTOs.Dataset.Normalized;

public record NormalizedNumericValueDto(
    double NormalizedValue,
    ParameterStateDto Parameter)
    : ParameterValueDto(
        NormalizedValue.ToString(),
        Parameter
    );
