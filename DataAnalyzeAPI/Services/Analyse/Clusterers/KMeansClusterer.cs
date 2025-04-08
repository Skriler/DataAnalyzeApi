using DataAnalyzeAPI.Models.Domain.Clustering;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Settings;
using DataAnalyzeAPI.Services.Analyse.DistanceCalculators;

namespace DataAnalyzeAPI.Services.Analyse.Clusterers;

public class KMeansClusterer : BaseClusterer<KMeansSettings>
{
    private readonly Random random = new();
    private List<KMeansCluster> clusters = new();

    public KMeansClusterer(IDistanceCalculator distanceCalculator)
        : base(distanceCalculator)
    { }

    public override List<Cluster> Cluster(DatasetModel dataset, KMeansSettings settings)
    {
        var parametersCount = dataset.Parameters.Count;

        if (dataset.Objects.Count < settings.NumberOfClusters)
            throw new InvalidOperationException("Objects amount is less than the number of clusters");

        clusters = new List<KMeansCluster>(settings.NumberOfClusters);
        InitializeClusters(dataset.Objects, settings.NumberOfClusters);

        for (int iteration = 0; iteration < settings.MaxIterations; ++iteration)
        {
            var assignmentsChanged = false;

            foreach (var cluster in clusters)
                cluster.Objects.Clear();

            for (int i = 0; i < dataset.Objects.Count; ++i)
            {
                var clusterIndex = GetNearestClusterIndex(dataset.Objects[i], settings);

                if (clusters[clusterIndex].Objects.Contains(dataset.Objects[i]))
                    continue;

                assignmentsChanged = true;
                clusters[clusterIndex].AddObject(dataset.Objects[i]);
            }

            if (!assignmentsChanged)
                break;

            RecalculateClusters(dataset.Objects);
        }


        return clusters
            .Cast<Cluster>()
            .ToList();
    }

    private void InitializeClusters(List<DataObjectModel> nodes, int numberOfClusters)
    {
        var selectedIndices = new HashSet<int>();

        for (int i = 0; i < numberOfClusters; ++i)
        {
            int randomIndex;
            do
            {
                randomIndex = random.Next(nodes.Count);
            } while (!selectedIndices.Add(randomIndex));

            var cluster = new KMeansCluster(nodes[randomIndex]);
            clusters.Add(cluster);
        }
    }

    private int GetNearestClusterIndex(DataObjectModel obj, KMeansSettings settings)
    {
        var clusterIndex = 0;
        var minDistance = double.MaxValue;

        for (int i = 0; i < clusters.Count; ++i)
        {
           var distance = distanceCalculator.Calculate(
               obj.Values,
               clusters[i].Centroid.Values,
               settings.NumericMetric,
               settings.CategoricalMetric);

            if (distance >= minDistance)
                continue;

            minDistance = distance;
            clusterIndex = i;
        }

        return clusterIndex;
    }

    private void RecalculateClusters(List<DataObjectModel> data)
    {
        foreach (var cluster in clusters)
        {
            cluster.RecalculateCentroid();
        }
    }
}
