using DataAnalyzeAPI.Models.Domain.Settings;
using DataAnalyzeAPI.Models.Enums;

namespace DataAnalyzeAPI.Services.Analyse.Clusterers;

public class ClustererFactory
{
    private readonly IServiceProvider serviceProvider;

    public ClustererFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public BaseClusterer<TSettings> Get<TSettings>(ClusterAlgorithm algorithm) where TSettings : IClusterSettings
    {
        return algorithm switch
        {
            ClusterAlgorithm.KMeans => serviceProvider.GetRequiredService<KMeansClusterer>() as BaseClusterer<TSettings>,
            ClusterAlgorithm.DBSCAN => serviceProvider.GetRequiredService<DBSCANClusterer>() as BaseClusterer<TSettings>,
            ClusterAlgorithm.HierarchicalAgglomerative => serviceProvider.GetRequiredService<AgglomerativeClusterer>() as BaseClusterer<TSettings>,
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm))
        };
    }
}
