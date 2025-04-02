using DataAnalyzeAPI.Models.Domain.Clustering;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Services.Analyse.Clusterers;

public interface ICluster
{
    List<Cluster> Cluster(DatasetModel dataset);
}
