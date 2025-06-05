namespace DataAnalyzeApi.Models.Domain.Dataset.Analysis;

public record DataObjectModel(
    long Id,
    string Name,
    List<ParameterValueModel> Values
);
