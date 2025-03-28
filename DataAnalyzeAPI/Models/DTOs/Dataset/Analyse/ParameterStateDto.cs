using DataAnalyzeAPI.Models.Enum;

namespace DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

public record ParameterStateDto(
    long Id,
    ParameterType Type,
    bool IsActive,
    double Weight
);
