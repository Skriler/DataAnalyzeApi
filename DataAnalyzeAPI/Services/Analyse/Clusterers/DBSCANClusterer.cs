using DataAnalyzeAPI.Models.Domain.Clustering;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Settings;
using DataAnalyzeAPI.Services.Analyse.DistanceCalculators;

namespace DataAnalyzeAPI.Services.Analyse.Clusterers;

public class DBSCANClusterer : BaseClusterer<DBSCANSettings>
{
    private readonly HashSet<DataObjectModel> visitedObjects = new();

    public DBSCANClusterer(IDistanceCalculator distanceCalculator)
        : base(distanceCalculator)
    { }

    public override List<Cluster> Cluster(DatasetModel dataset, DBSCANSettings settings)
    {
        var clusters = new List<Cluster>();
        visitedObjects.Clear();

        foreach (var obj in dataset.Objects)
        {
            if (visitedObjects.Contains(obj))
                continue;

            visitedObjects.Add(obj);

            var neighbors = GetNeighbors(obj, dataset.Objects, settings);

            if (neighbors.Count < settings.MinPoints)
                continue;

            var cluster = new Cluster();
            clusters.Add(cluster);

            ExpandCluster(cluster, obj, neighbors, dataset.Objects, settings);
        }

        return clusters;
    }

    private List<DataObjectModel> GetNeighbors(
        DataObjectModel obj,
        List<DataObjectModel> objects,
        DBSCANSettings settings)
    {
        return objects
            .Where(other => IsNeighbor(obj, other, settings))
            .ToList();
    }

    private bool IsNeighbor(DataObjectModel obj, DataObjectModel other, DBSCANSettings settings)
    {
        var distance = distanceCalculator.Calculate(
            obj.Values,
            other.Values,
            settings.NumericMetric,
            settings.CategoricalMetric);

        return distance <= settings.Epsilon;
    }

    private void ExpandCluster(
        Cluster cluster,
        DataObjectModel obj,
        List<DataObjectModel> neighbors,
        List<DataObjectModel> objects,
        DBSCANSettings settings)
    {
        cluster.AddObject(obj);

        var сonnectedNodes = new Queue<DataObjectModel>(neighbors);

        while (сonnectedNodes.Count > 0)
        {
            var neighbor = сonnectedNodes.Dequeue();

            if (visitedObjects.Contains(neighbor))
                continue;

            visitedObjects.Add(neighbor);
            var neighborNeighbors = GetNeighbors(neighbor, objects, settings);


            if (neighborNeighbors.Count >= settings.MinPoints)
            {
                foreach (var newNeighbor in neighborNeighbors)
                {
                    if (!cluster.Objects.Contains(newNeighbor) && !сonnectedNodes.Contains(newNeighbor))
                    {
                        сonnectedNodes.Enqueue(newNeighbor);
                    }
                }
            }

            if (!cluster.Objects.Contains(neighbor))
            {
                cluster.AddObject(neighbor);
            }
        }
    }
}
