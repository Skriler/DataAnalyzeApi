using DataAnalyzeApi.Models.Enum;

namespace DataAnalyzeApi.Models.Domain.Dataset.Analyse;

public record ParameterStateModel(
    long Id,
    string Name,
    ParameterType Type,
    bool IsActive,
    double Weight
);
