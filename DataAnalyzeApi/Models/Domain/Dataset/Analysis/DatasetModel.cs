namespace DataAnalyzeApi.Models.Domain.Dataset.Analysis;

public record DatasetModel(
    long Id,
    string Name,
    List<ParameterStateModel> Parameters,
    List<DataObjectModel> Objects
);
