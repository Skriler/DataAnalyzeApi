using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;

namespace DataAnalyzeApi.Mappers.Analysis.Entities;

public class ClusteringEntityAnalysisMapper : BaseEntityAnalysisMapper<ClusteringAnalysisResult, ClusteringAnalysisResultDto>
{
    /// <summary>
    /// Maps ClusteringAnalysisResult to ClusteringAnalysisResultDto.
    /// </summary>
    public override ClusteringAnalysisResultDto MapAnalysisResult(
        ClusteringAnalysisResult result,
        bool includeParameters = false)
    {
        return new ClusteringAnalysisResultDto
        {
            DatasetId = result.DatasetId,
            Algorithm = result.Algorithm,
            Clusters = MapClusterList(result.Clusters, includeParameters),
        };
    }

    /// <summary>
    /// Maps Cluster entity list to ClusterDto list.
    /// </summary>
    public virtual List<ClusterDto> MapClusterList(
        List<Cluster> clusters,
        bool includeParameters = false)
        => clusters.ConvertAll(c => MapCluster(c, includeParameters));

    /// <summary>
    /// Maps Cluster entity to ClusterDto.
    /// </summary>
    public virtual ClusterDto MapCluster(
        Cluster cluster,
        bool includeParameters = false)
    {
        var objectsDto = cluster.Objects
            .ConvertAll(obj => MapDataObject(obj, includeParameters));

        return new ClusterDto($"Cluster {cluster.Id}", objectsDto);
    }
}
