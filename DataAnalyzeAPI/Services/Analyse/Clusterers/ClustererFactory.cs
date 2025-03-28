using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Services.Analyse.Clusterers;

public class ClustererFactory
{
    private readonly IServiceProvider serviceProvider;

    public ClustererFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public ICluster Get(ClusterAlgorithm algorithm)
    {
        return algorithm switch
        {
            ClusterAlgorithm.KMeans => serviceProvider.GetRequiredService<KMeansClusterer>(),
            ClusterAlgorithm.DBSCAN => serviceProvider.GetRequiredService<DBSCANClusterer>(),
            ClusterAlgorithm.HierarchicalAgglomerative => serviceProvider.GetRequiredService<AgglomerativeClusterer>(),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm))
        };
    }
}
