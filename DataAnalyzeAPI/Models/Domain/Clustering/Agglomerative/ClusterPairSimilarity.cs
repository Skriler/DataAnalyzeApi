namespace DataAnalyzeApi.Models.Domain.Clustering.Agglomerative;

public class ClusterPairSimilarity
{
    public int ClusterAId { get; private set; }

    public int ClusterBId { get; private set; }

    public double Similarity { get; private set; }

    public ClusterPairSimilarity()
    {
        Similarity = double.MaxValue;
    }

    public void Update(int clusterAId, int clusterBId, double similarity)
    {
        ClusterAId = clusterAId;
        ClusterBId = clusterBId;
        Similarity = similarity;
    }
}
