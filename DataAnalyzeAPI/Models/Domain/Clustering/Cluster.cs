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
}
