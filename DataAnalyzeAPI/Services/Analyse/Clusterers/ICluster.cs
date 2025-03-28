using DataAnalyzeAPI.Models.DTOs.Analyse.Clusters;
using DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

namespace DataAnalyzeAPI.Services.Analyse.Clusterers;

public interface ICluster
{
    List<ClusterDto> Cluster(DatasetDto dataset);
}
