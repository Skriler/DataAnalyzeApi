using DataAnalyzeAPI.Models.Enum;

namespace DataAnalyzeAPI.Models.DTOs.Dataset.Analysis;

public record ParameterStateDto(
    long Id,
    ParameterType Type,
    bool IsActive,
    double Weight
);
