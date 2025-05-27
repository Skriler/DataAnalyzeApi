using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Clustering.KMeans;

public record Centroid(
    long Id,
    string Name,
    List<ParameterValueModel> Values
) : DataObjectModel(
    Id,
    Name,
    Values
);
