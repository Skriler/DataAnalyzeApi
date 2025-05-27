using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;

namespace DataAnalyzeApi.Services.Analyse.Factories.Clusterer;

public interface IClustererFactory
{
    BaseClusterer<TSettings> Get<TSettings>(ClusterAlgorithm algorithm) where TSettings : BaseClusterSettings;
}
