using DataAnalyzeApi.Exceptions;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;

namespace DataAnalyzeApi.Services.Analysis.Factories.Clusterer;

public class ClustererFactory(IServiceProvider serviceProvider) : IClustererFactory
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public BaseClusterer<TSettings> Get<TSettings>(ClusteringAlgorithm algorithm)
        where TSettings : BaseClusterSettings
    {
        var clusterer = algorithm switch
        {
            ClusteringAlgorithm.KMeans => serviceProvider.GetRequiredService<KMeansClusterer>() as BaseClusterer<TSettings>,
            ClusteringAlgorithm.DBSCAN => serviceProvider.GetRequiredService<DBSCANClusterer>() as BaseClusterer<TSettings>,
            ClusteringAlgorithm.Agglomerative => serviceProvider.GetRequiredService<AgglomerativeClusterer>() as BaseClusterer<TSettings>,
            _ => throw new TypeNotFoundException(nameof(algorithm))
        };

        return clusterer ?? throw new InvalidCastException($"Cannot cast clusterer to BaseClusterer<{typeof(TSettings).Name}>");
    }
}
