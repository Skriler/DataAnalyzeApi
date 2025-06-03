using DataAnalyzeApi.Models.Domain.Dataset.Analysis;

namespace DataAnalyzeApi.Models.Domain.Dataset;

public record DataObjectModel(
    long Id,
    string Name,
    List<ParameterValueModel> Values
);
