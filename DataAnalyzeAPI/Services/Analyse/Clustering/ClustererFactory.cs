using DataAnalyzeApi.Exceptions;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;

namespace DataAnalyzeApi.Services.Analyse.Clusterers;

public class ClustererFactory(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public BaseClusterer<TSettings> Get<TSettings>(ClusterAlgorithm algorithm) where TSettings : IClusterSettings
    {
        return algorithm switch
        {
            ClusterAlgorithm.KMeans => serviceProvider.GetRequiredService<KMeansClusterer>() as BaseClusterer<TSettings>,
            ClusterAlgorithm.DBSCAN => serviceProvider.GetRequiredService<DBSCANClusterer>() as BaseClusterer<TSettings>,
            ClusterAlgorithm.Agglomerative => serviceProvider.GetRequiredService<AgglomerativeClusterer>() as BaseClusterer<TSettings>,
            _ => throw new TypeNotFoundException(nameof(algorithm))
        };
    }
}
