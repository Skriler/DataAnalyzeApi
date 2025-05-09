using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Clustering.KMeans;

public class KMeansCluster : Cluster
{
    private static int nextId = 0;

    public Centroid Centroid { get; set; }

    public int Id { get; }

    public KMeansCluster(DataObjectModel obj, string name)
        : base(name)
    {
        Id = nextId++;

        var valuesCopy = obj.Values.ConvertAll(v => v.DeepClone());
        Centroid = new Centroid(Id, name, valuesCopy);
    }
}
