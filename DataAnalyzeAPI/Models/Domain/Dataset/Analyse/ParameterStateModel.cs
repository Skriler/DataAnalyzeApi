using DataAnalyzeAPI.Models.Enum;

namespace DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

public record ParameterStateModel(
    long Id,
    string Name,
    ParameterType Type,
    bool IsActive,
    double Weight
);
