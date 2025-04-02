namespace DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

public record DatasetModel(
    long Id,
    string Name,
    List<ParameterStateModel> Parameters,
    List<DataObjectModel> Objects
);
