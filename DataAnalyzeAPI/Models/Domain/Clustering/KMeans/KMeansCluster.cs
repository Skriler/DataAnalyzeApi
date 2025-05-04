using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Clustering.KMeans;

public class KMeansCluster(DataObjectModel obj, string name) : Cluster(name)
{
    public Centroid Centroid { get; set; } = new Centroid(obj.Values);
}
