using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;

namespace DataAnalyzeApi.Services.Analysis.Factories.Clusterer;

public interface IClustererFactory
{
    BaseClusterer<TSettings> Get<TSettings>(ClusteringAlgorithm algorithm) where TSettings : BaseClusterSettings;
}
