using DataAnalyzeApi.Models.Domain.Dataset.Analysis;

namespace DataAnalyzeApi.Models.Domain.Clustering.KMeans;

public class KMeansClusterModel : ClusterModel
{
    public Centroid Centroid { get; set; }

    public KMeansClusterModel(DataObjectModel obj, string name)
        : base(name)
    {
        var valuesCopy = obj.Values.ConvertAll(v => v.DeepClone());
        Centroid = new Centroid(obj.Id, name + "_centroid", valuesCopy);
    }
}
