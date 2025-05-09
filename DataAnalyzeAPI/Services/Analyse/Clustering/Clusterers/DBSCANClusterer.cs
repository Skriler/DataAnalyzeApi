using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;

namespace DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;

public class DBSCANClusterer : BaseClusterer<DBSCANSettings>
{
    protected override string ClusterPrefix => nameof(ClusterAlgorithm.DBSCAN);
    protected string NoiseClusterPrefix => "Noise";

    private readonly ClusterNameGenerator nameGenerator;

    /// <summary>
    /// A set of visited objects to prevent reprocessing.
    /// </summary>
    private readonly HashSet<DataObjectModel> visitedObjects = new();

    /// <summary>
    /// A set of objects classified as noise.
    /// </summary>
    private readonly HashSet<DataObjectModel> noiseObjects = new();

    private DBSCANSettings settings = default!;

    public DBSCANClusterer(
        IDistanceCalculator distanceCalculator,
        ClusterNameGenerator nameGenerator
        ) : base(distanceCalculator)
    {
        this.nameGenerator = nameGenerator;
    }

    /// <summary>
    /// Performs a clustering process by iterating over all objects.
    /// For each unvisited object, it checks if it has enough neighbors to form a cluster.
    /// If it does, a new cluster is created, and the cluster is expanded by adding neighboring objects.
    /// Objects with insufficient neighbors are marked as noise.
    /// </summary>
    public override List<Cluster> Cluster(List<DataObjectModel> objects, DBSCANSettings settings)
    {
        this.settings = settings;
        var clusters = new List<Cluster>();

        foreach (var obj in objects)
        {
            if (visitedObjects.Contains(obj))
                continue;

            visitedObjects.Add(obj);

            var neighbors = GetNeighbors(obj, objects);

            if (neighbors.Count < settings.MinPoints)
            {
                noiseObjects.Add(obj);
                continue;
            }

            var cluster = new Cluster(nameGenerator.GenerateName(ClusterPrefix));
            cluster.AddObject(obj);
            noiseObjects.Remove(obj);

            clusters.Add(cluster);

            ExpandCluster(cluster, neighbors, objects);
        }

        AddNoiseCluster(clusters);

        return clusters
            .OrderByDescending(c => c.Objects.Count)
            .ToList();
    }

    /// <summary>
    /// Retrieves the neighbors of a given object from the list of all objects.
    /// </summary>
    private List<DataObjectModel> GetNeighbors(
        DataObjectModel obj,
        List<DataObjectModel> objects)
    {
        return objects
            .Where(other => other != obj && IsNeighbor(obj, other))
            .ToList();
    }

    /// <summary>
    /// Checks if two objects are neighbors based on the distance metric.
    /// </summary>
    private bool IsNeighbor(DataObjectModel obj, DataObjectModel other)
    {
        var distance = distanceCalculator.Calculate(
            obj,
            other,
            settings.NumericMetric,
            settings.CategoricalMetric);

        return distance <= settings.Epsilon;
    }

    /// <summary>
    /// Expands the cluster by adding neighbors
    /// and their neighbors recursively.
    /// </summary>
    private void ExpandCluster(
        Cluster cluster,
        List<DataObjectModel> neighbors,
        List<DataObjectModel> objects)
    {
        var neighborsToProcess = new Queue<DataObjectModel>(neighbors);

        while (neighborsToProcess.Count > 0)
        {
            var currentNeighbor = neighborsToProcess.Dequeue();

            if (visitedObjects.Contains(currentNeighbor))
                continue;

            cluster.AddObject(currentNeighbor);
            visitedObjects.Add(currentNeighbor);
            noiseObjects.Remove(currentNeighbor);

            var currentNeighbors = GetNeighbors(currentNeighbor, objects);

            if (currentNeighbors.Count < settings.MinPoints)
                continue;

            foreach (var neighbor in currentNeighbors)
            {
                if (visitedObjects.Contains(neighbor) ||
                    neighborsToProcess.Contains(neighbor))
                {
                    continue;
                }

                neighborsToProcess.Enqueue(neighbor);
            }
        }
    }

    /// <summary>
    /// Adds a noise cluster to the list of clusters
    /// if there are any noise objects.
    /// </summary>
    private void AddNoiseCluster(List<Cluster> clusters)
    {
        if (noiseObjects.Count == 0)
            return;

        var noiseCluster = new Cluster(nameGenerator.GenerateName(NoiseClusterPrefix));
        noiseCluster.AddObjects(noiseObjects.ToList());

        clusters.Add(noiseCluster);
    }
}
