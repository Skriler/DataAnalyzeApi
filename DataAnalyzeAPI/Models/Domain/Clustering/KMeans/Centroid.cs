using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Clustering.KMeans;

public class Centroid(List<ParameterValueModel> values)
{
    public List<ParameterValueModel> Values { get; } = values.ConvertAll(v => v.DeepClone());
}
