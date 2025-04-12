using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.Domain.Clustering.KMeans;

public class KMeansCluster : Cluster
{
    public Centroid Centroid { get; set; }

    public KMeansCluster(DataObjectModel obj, string name)
        : base(name)
    {
        Centroid = new Centroid(obj);
    }

    public void RecalculateCentroid()
    {
        Centroid.Recalculate(Objects);
    }
}
