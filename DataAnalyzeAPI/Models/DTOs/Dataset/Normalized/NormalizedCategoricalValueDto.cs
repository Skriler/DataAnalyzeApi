using DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.DTOs.Dataset.Normalized;

public record NormalizedCategoricalValueDto(
    int[] OneHotValues,
    ParameterStateDto Parameter)
    : ParameterValueDto(
        string.Join(", ", OneHotValues),
        Parameter
    );
