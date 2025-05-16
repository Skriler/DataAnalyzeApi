using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Clustering;

public class Cluster(string name)
{
    public string Name { get; } = name;

    public List<DataObjectModel> Objects { get; set; } = new();

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
