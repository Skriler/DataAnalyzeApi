using DataAnalyzeApi.Models.DTOs.Analysis.Clustering;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities;
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
        var coordinatesDict = result.ObjectCoordinates
            .ToDictionary(coord => coord.ObjectId, coord => coord);

        return new ClusteringAnalysisResultDto
        {
            DatasetId = result.DatasetId,
            Algorithm = result.Algorithm,
            Clusters = MapClusterList(
                result.Clusters,
                coordinatesDict,
                includeParameters),
        };
    }

    /// <summary>
    /// Maps Cluster entity list to ClusterDto list.
    /// </summary>
    public virtual List<ClusterDto> MapClusterList(
        List<Cluster> clusters,
        Dictionary<long, DataObjectCoordinate> coordinatesDict,
        bool includeParameters = false) =>
        clusters.ConvertAll(c => MapCluster(c, coordinatesDict, includeParameters));

    /// <summary>
    /// Maps Cluster entity to ClusterDto.
    /// </summary>
    public virtual ClusterDto MapCluster(
        Cluster cluster,
        Dictionary<long, DataObjectCoordinate> coordinatesDict,
        bool includeParameters = false)
    {
        var objectsDto = cluster.Objects
            .ConvertAll(obj => MapDataObjectForClustering(obj, coordinatesDict, includeParameters));

        return new ClusterDto
        {
            Name = $"Cluster {cluster.Id}",
            Objects = objectsDto
        };
    }

    /// <summary>
    /// Maps DataObject entity to DataObjectClusteringAnalysisDto with coordinates.
    /// </summary>
    protected virtual DataObjectClusteringAnalysisDto MapDataObjectForClustering(
        DataObject dataObject,
        Dictionary<long, DataObjectCoordinate> coordinatesDict,
        bool includeParameters)
    {
        var parameterValues = MapParameterValues(dataObject.Values, includeParameters);
        coordinatesDict.TryGetValue(dataObject.Id, out var coords);

        return new DataObjectClusteringAnalysisDto
        {
            Id = dataObject.Id,
            Name = dataObject.Name,
            ParameterValues = parameterValues!,
            X = coords?.X ?? 0.0,
            Y = coords?.Y ?? 0.0
        };
    }
}
