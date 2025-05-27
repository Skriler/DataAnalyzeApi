using DataAnalyzeApi.Exceptions;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;

namespace DataAnalyzeApi.Services.Analyse.Factories.Clusterer;

public class ClustererFactory(IServiceProvider serviceProvider) : IClustererFactory
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public BaseClusterer<TSettings> Get<TSettings>(ClusterAlgorithm algorithm)
        where TSettings : BaseClusterSettings
    {
        var clusterer = algorithm switch
        {
            ClusterAlgorithm.KMeans => serviceProvider.GetRequiredService<KMeansClusterer>() as BaseClusterer<TSettings>,
            ClusterAlgorithm.DBSCAN => serviceProvider.GetRequiredService<DBSCANClusterer>() as BaseClusterer<TSettings>,
            ClusterAlgorithm.Agglomerative => serviceProvider.GetRequiredService<AgglomerativeClusterer>() as BaseClusterer<TSettings>,
            _ => throw new TypeNotFoundException(nameof(algorithm))
        };

        return clusterer ?? throw new InvalidCastException($"Cannot cast clusterer to BaseClusterer<{typeof(TSettings).Name}>");
    }
}
