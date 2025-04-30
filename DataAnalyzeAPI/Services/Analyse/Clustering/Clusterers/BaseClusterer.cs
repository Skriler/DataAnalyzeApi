using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;

namespace DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;

public abstract class BaseClusterer<TSettings>(IDistanceCalculator distanceCalculator)
    where TSettings : IClusterSettings
{
    protected readonly IDistanceCalculator distanceCalculator = distanceCalculator;

    /// <summary>
    /// Prefix for cluster names.
    /// </summary>
    protected abstract string ClusterPrefix { get; }

    public abstract List<Cluster> Cluster(List<DataObjectModel> Objects, TSettings settings);
}
