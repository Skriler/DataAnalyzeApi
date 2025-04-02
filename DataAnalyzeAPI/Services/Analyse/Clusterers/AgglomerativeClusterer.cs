using DataAnalyzeAPI.Models.Domain.Clustering;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Services.Analyse.DistanceCalculators;

namespace DataAnalyzeAPI.Services.Analyse.Clusterers;

public class AgglomerativeClusterer : ICluster
{
    private readonly IDistanceCalculator distanceCalculator;

    private List<AgglomerativeCluster> clusters = new();

    public AgglomerativeClusterer(IDistanceCalculator distanceCalculator)
    {
        this.distanceCalculator = distanceCalculator;
    }

    public List<Cluster> Cluster(DatasetModel dataset)
    {
        clusters = dataset.Objects
            .ConvertAll(obj => new AgglomerativeCluster(obj));

        while (clusters.Count(c => !c.IsMerged) > 1)
        {
            var mostSimilarPair = FindMostSimilarClusters();

            // TODO add threshold
            if (mostSimilarPair.Similarity > 0.5)
                break;

            clusters[mostSimilarPair.ClusterAId]
                .Merge(clusters[mostSimilarPair.ClusterBId]);
        }

        return clusters
            .Where(c => !c.IsMerged)
            .Cast<Cluster>()
            .ToList();
    }

    private ClusterPairSimilarity FindMostSimilarClusters()
    {
        var clusterSimilarity = new ClusterPairSimilarity();

        for (int i = 0; i < clusters.Count; ++i)
        {
            if (clusters[i].IsMerged)
                continue;

            for (int j = i + 1; j < clusters.Count; ++j)
            {
                if (clusters[j].IsMerged)
                    continue;

                var similarity = GetAverageDistance(
                    clusters[i],
                    clusters[j]
                    );

                if (similarity > clusterSimilarity.Similarity)
                    continue;

                clusterSimilarity.Update(i, j, similarity);
            }
        }

        return clusterSimilarity;
    }

    private double GetAverageDistance(AgglomerativeCluster clusterA, AgglomerativeCluster clusterB)
    {
        return clusterA.Objects
            .SelectMany(objA => clusterB.Objects, distanceCalculator.Calculate)
            .Average();
    }
}
