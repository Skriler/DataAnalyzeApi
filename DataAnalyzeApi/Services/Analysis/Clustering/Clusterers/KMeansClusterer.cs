using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Clustering.KMeans;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Clustering.Helpers;
using DataAnalyzeApi.Services.Analysis.DistanceCalculators;

namespace DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;

public class KMeansClusterer(
    IDistanceCalculator distanceCalculator,
    ClusterNameGenerator nameGenerator,
    CentroidCalculator centroidCalculator
    ) : BaseClusterer<KMeansSettings>(distanceCalculator, nameGenerator)
{
    protected override string ClusterPrefix => nameof(ClusterAlgorithm.KMeans);

    private readonly CentroidCalculator centroidCalculator = centroidCalculator;

    private List<KMeansClusterModel> clusters = default!;
    private KMeansSettings settings = default!;

    /// <summary>
    /// A dictionary that maps each object to the index of the cluster it is assigned to.
    /// Used to track the current cluster assignment of each object, enabling the detection
    /// of changes in object assignments during the clustering process.
    /// </summary>
    private Dictionary<DataObjectModel, int> objectClusterMap = [];

    public override List<ClusterModel> Cluster(List<DataObjectModel> objects, KMeansSettings settings)
    {
        Validate(objects, settings);

        this.settings = settings;
        clusters = new List<KMeansClusterModel>(settings.NumberOfClusters);
        objectClusterMap = new Dictionary<DataObjectModel, int>(objects.Count);

        InitializeClusters(objects);
        PerformClustering(objects);

        return clusters
            .Cast<ClusterModel>()
            .OrderByDescending(c => c.Objects.Count)
            .ToList();
    }

    /// <summary>
    /// Initializes the clusters by selecting random objects as the initial centroids.
    /// </summary>
    private void InitializeClusters(List<DataObjectModel> objects)
    {
        // For n clusters, we want to divide the range [0, objects.Count-1] into n equal parts
        // and take the middle point of each part as the centroid index
        for (int i = 0; i < settings.NumberOfClusters; ++i)
        {
            double segmentStart = (double)i * objects.Count / settings.NumberOfClusters;
            double segmentEnd = (double)(i + 1) * objects.Count / settings.NumberOfClusters;

            int middleIndex = (int)Math.Floor((segmentStart + segmentEnd) / 2);

            middleIndex = Math.Max(0, Math.Min(middleIndex, objects.Count - 1));

            var cluster = new KMeansClusterModel(objects[middleIndex], nameGenerator.GenerateName(ClusterPrefix));
            clusters.Add(cluster);
        }
    }

    /// <summary>
    /// Performs a clustering process, assigning objects to clusters
    /// and recalculating centroids until convergence
    /// or the maximum number of iterations is reached.
    /// </summary>
    private void PerformClustering(List<DataObjectModel> objects)
    {
        for (int iteration = 0; iteration < settings.MaxIterations; ++iteration)
        {
            clusters.ForEach(c => c.Objects.Clear());

            if (!TryAssignObjectsToClusters(objects))
                break;

            RecalculateCentroids();
        }
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
                obj,
                clusters[i].Centroid,
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
    /// Recalculates the centroid of each cluster based on the current set of assigned objects.
    /// </summary>
    public void RecalculateCentroids()
    {
        foreach (var cluster in clusters)
        {
            cluster.Centroid = centroidCalculator.Recalculate(cluster.Centroid, cluster.Objects);
        }
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
