using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Clustering.Agglomerative;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Clustering.Helpers;
using DataAnalyzeApi.Services.Analysis.DistanceCalculators;

namespace DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;

public class AgglomerativeClusterer(
    IDistanceCalculator distanceCalculator,
    ClusterNameGenerator nameGenerator
    ) : BaseClusterer<AgglomerativeSettings>(distanceCalculator, nameGenerator)
{
    protected override string ClusterPrefix => nameof(ClusterAlgorithm.Agglomerative);

    private List<AgglomerativeCluster> clusters = default!;
    private AgglomerativeSettings settings = default!;

    public override List<Cluster> Cluster(List<DataObjectModel> objects, AgglomerativeSettings settings)
    {
        this.settings = settings;
        clusters = objects.ConvertAll(
            obj => new AgglomerativeCluster(obj, nameGenerator.GenerateName(ClusterPrefix))
            );

        PerformClustering();

        return clusters
            .Where(c => !c.IsMerged)
            .OrderByDescending(c => c.Objects.Count)
            .Cast<Cluster>()
            .ToList();
    }

    /// <summary>
    /// Performs a clustering process, merging the most similar clusters
    /// until convergence or a threshold is reached.
    /// </summary>
    private void PerformClustering()
    {
        while (clusters.Count(c => !c.IsMerged) > 1)
        {
            var mostSimilarPair = FindMostSimilarClusters();

            if (mostSimilarPair.Similarity > settings.Threshold)
                break;

            clusters[mostSimilarPair.ClusterAId]
                .Merge(clusters[mostSimilarPair.ClusterBId]);
        }
    }

    /// <summary>
    /// Finds the cluster pair with the highest similarity.
    /// </summary>
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

                var similarity = GetAverageDistance(clusters[i], clusters[j]);

                if (similarity > clusterSimilarity.Similarity)
                    continue;

                clusterSimilarity.Update(i, j, similarity);
            }
        }

        return clusterSimilarity;
    }

    /// <summary>
    /// Calculates the average distance between two clusters based on their objects.
    /// </summary>
    private double GetAverageDistance(AgglomerativeCluster clusterA, AgglomerativeCluster clusterB)
    {
        var distances = new List<double>();

        foreach (var objA in clusterA.Objects)
        {
            foreach (var objB in clusterB.Objects)
            {
                var distance = distanceCalculator.Calculate(
                    objA,
                    objB,
                    settings.NumericMetric,
                    settings.CategoricalMetric);

                distances.Add(distance);
            }
        }

        return distances.Average();
    }
}
