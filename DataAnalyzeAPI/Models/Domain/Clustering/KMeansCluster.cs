using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.Domain.Clustering;

public class KMeansCluster : Cluster
{
    public Centroid Centroid { get; set; }

    public KMeansCluster(DataObjectModel obj)
    {
        Centroid = new Centroid(obj);
    }

    public void RecalculateCentroid()
    {
        Centroid.Recalculate(Objects);
    }
}
