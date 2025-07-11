using DataAnalyzeApi.Models.Domain.Dataset.Analysis;

namespace DataAnalyzeApi.Models.Domain.Clustering.Agglomerative;

public class AgglomerativeClusterModel : ClusterModel
{
    public bool IsMerged { get; private set; }

    public AgglomerativeClusterModel(DataObjectModel obj, string name)
        : base(name)
    {
        Objects.Add(obj);
        IsMerged = false;
    }

    public void Merge(AgglomerativeClusterModel mergedCluster)
    {
        Objects.AddRange(mergedCluster.Objects);
        mergedCluster.IsMerged = true;
    }
}
