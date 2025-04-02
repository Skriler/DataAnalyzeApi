namespace DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

public record DataObjectModel(
    long Id,
    string Name,
    List<ParameterValueModel> Values
);
