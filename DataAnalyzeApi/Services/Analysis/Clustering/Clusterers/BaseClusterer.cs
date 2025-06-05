using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Services.Analysis.Clustering.Helpers;
using DataAnalyzeApi.Services.Analysis.DistanceCalculators;

namespace DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;

public abstract class BaseClusterer<TSettings>(
    IDistanceCalculator distanceCalculator,
    ClusterNameGenerator clusterNameGenerator
    ) where TSettings : BaseClusterSettings
{
    protected readonly IDistanceCalculator distanceCalculator = distanceCalculator;
    protected readonly ClusterNameGenerator nameGenerator = clusterNameGenerator;

    /// <summary>
    /// Prefix for cluster names.
    /// </summary>
    protected abstract string ClusterPrefix { get; }

    public abstract List<Cluster> Cluster(List<DataObjectModel> Objects, TSettings settings);
}
