using DataAnalyzeAPI.Models.Domain.Clustering;
using DataAnalyzeAPI.Models.Domain.Clustering.KMeans;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Settings;
using DataAnalyzeAPI.Services.Analyse.DistanceCalculators;
using DataAnalyzeAPI.Services.Helpers;

namespace DataAnalyzeAPI.Services.Analyse.Clusterers;

public class KMeansClusterer : BaseClusterer<KMeansSettings>
{
    protected override string ClusterPrefix => "KMeans";

    private readonly ClusterNameGenerator nameGenerator;
    private readonly Random random = new();

    private List<KMeansCluster> clusters = new();
    private KMeansSettings settings = default!;

    /// <summary>
    /// A dictionary that maps each object to the index of the cluster it is assigned to.
    /// Used to track the current cluster assignment of each object, enabling the detection
    /// of changes in object assignments during the clustering process.
    /// </summary>
    private Dictionary<DataObjectModel, int> objectClusterMap = new();

    public KMeansClusterer(
        IDistanceCalculator distanceCalculator,
        ClusterNameGenerator nameGenerator
        ) : base(distanceCalculator)
    {
        this.nameGenerator = nameGenerator;
    }

    public override List<Cluster> Cluster(List<DataObjectModel> objects, KMeansSettings settings)
    {
        Validate(objects, settings);

        this.settings = settings;
        clusters = new List<KMeansCluster>(settings.NumberOfClusters);
        objectClusterMap = new Dictionary<DataObjectModel, int>(objects.Count);

        InitializeClusters(objects);
        return PerformClustering(objects);
    }

    /// <summary>
    /// Initializes the clusters by selecting random objects as the initial centroids.
    /// </summary>
    private void InitializeClusters(List<DataObjectModel> objects)
    {
        var selectedIndices = new HashSet<int>();

        var randomIndices = Enumerable.Range(0, objects.Count)
            .OrderBy(_ => random.Next())
            .Take(settings.NumberOfClusters)
            .ToList();

        foreach (var index in randomIndices)
        {
            var cluster = new KMeansCluster(objects[index], nameGenerator.GenerateName(ClusterPrefix));
            clusters.Add(cluster);
        }
    }

    /// <summary>
    /// Performs a clustering process, assigning objects to clusters
    /// and recalculating centroids until convergence
    /// or the maximum number of iterations is reached.
    /// </summary>
    private List<Cluster> PerformClustering(List<DataObjectModel> objects)
    {
        for (int iteration = 0; iteration < settings.MaxIterations; ++iteration)
        {
            clusters.ForEach(c => c.Objects.Clear());

            if (!TryAssignObjectsToClusters(objects))
                break;

            clusters.ForEach(c => c.RecalculateCentroid());
        }

        return clusters
            .Cast<Cluster>()
            .OrderByDescending(c => c.Objects.Count)
            .ToList();
    }

    /// <summary>
    /// Attempts to assign each object to the nearest cluster.
    /// Returns true if any object assignment changes.
    /// </summary>
    private bool TryAssignObjectsToClusters(List<DataObjectModel> objects)
    {
        var assignmentsChanged = false;

        foreach (var obj in objects)
        {
            int nearestClusterIndex = GetNearestClusterIndex(obj);
            clusters[nearestClusterIndex].AddObject(obj);

            if (objectClusterMap.TryGetValue(obj, out int previousClusterIndex) ||
                previousClusterIndex != nearestClusterIndex)
            {
                continue;
            }

            objectClusterMap[obj] = nearestClusterIndex;
            assignmentsChanged = true;
        }

        return assignmentsChanged;
    }

    /// <summary>
    /// Calculates the index of the cluster that is closest to the object
    /// based on the distance to the cluster's centroid.
    /// </summary>
    private int GetNearestClusterIndex(DataObjectModel obj)
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

    /// <summary>
    /// Validates input parameters.
    /// Throws an exception if there are fewer objects than clusters.
    /// </summary>
    private static void Validate(List<DataObjectModel> Objects, KMeansSettings settings)
    {
        if (Objects.Count < settings.NumberOfClusters)
            throw new InvalidOperationException("Objects amount is less than the number of clusters");
    }
}
