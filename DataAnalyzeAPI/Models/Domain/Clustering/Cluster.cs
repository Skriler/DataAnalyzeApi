using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.Domain.Clustering;

public class Cluster
{
    private static int nextId = 0;

    public int Id { get; }

    public List<DataObjectModel> Objects { get; } = new();

    public Cluster()
    {
        Id = nextId++;
    }

    public void AddObject(DataObjectModel obj)
    {
        if (Objects.Contains(obj))
            return;

        Objects.Add(obj);
    }
}
