using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Clustering.KMeans;

public class Centroid
{
    public List<ParameterValueModel> Values { get; }

    public Centroid(DataObjectModel obj)
        : this(obj.Values)
    { }

    public Centroid(List<ParameterValueModel> values)
    {
        Values = values.ConvertAll(v => v.DeepClone());
    }
}
