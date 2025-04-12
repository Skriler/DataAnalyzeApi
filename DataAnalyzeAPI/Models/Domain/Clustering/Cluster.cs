using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.Domain.Clustering;

public class Cluster
{
    public string Name { get; }

    public List<DataObjectModel> Objects { get; } = new();

    public Cluster(string name)
    {
        Name = name;
    }

    public void AddObject(DataObjectModel obj)
    {
        if (Objects.Contains(obj))
            return;

        Objects.Add(obj);
    }

    public void AddObjects(List<DataObjectModel> objects)
    {
        foreach(var obj in objects)
        {
            AddObject(obj);
        }
    }
}
