using DataAnalyzeApi.Models.Domain.Dataset.Analysis;

namespace DataAnalyzeApi.Models.Domain.Clustering.Agglomerative;

public class AgglomerativeCluster : Cluster
{
    public bool IsMerged { get; private set; }

    public AgglomerativeCluster(DataObjectModel obj, string name)
        : base(name)
    {
        Objects.Add(obj);
        IsMerged = false;
    }

    public void Merge(AgglomerativeCluster mergedCluster)
    {
        Objects.AddRange(mergedCluster.Objects);
        mergedCluster.IsMerged = true;
    }
}
