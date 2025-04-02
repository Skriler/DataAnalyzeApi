namespace DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

public record ParameterValueModel(
    string Value,
    ParameterStateModel Parameter
);
