using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.DimensionalityReduction;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;

namespace DataAnalyzeApi.Mappers.Analysis.Domain;

/// <summary>
/// Mapper for converting clustering domain models to analysis DTOs.
/// </summary>
public class ClusteringDomainAnalysisMapper : BaseDomainAnalysisMapper
{
    /// <summary>
    /// Maps ClusterModel to its DTO.
    /// </summary>
    public virtual ClusterDto Map(
        ClusterModel cluster,
        List<DataObjectCoordinateModel> coordinateModels,
        bool includeParameters = false)
    {
        var objectsDto = cluster.Objects
            .ConvertAll(obj => MapDataObjectForClustering(obj, coordinateModels, includeParameters));

        return new ClusterDto
        {
            Name = cluster.Name,
            Objects = objectsDto
        };
    }

    /// <summary>
    /// Maps ClusterModel list to their DTOs.
    /// </summary>
    public virtual List<ClusterDto> MapList(
        List<ClusterModel> clusters,
        List<DataObjectCoordinateModel> coordinateModels,
        bool includeParameters = false) =>
        clusters.ConvertAll(c => Map(c, coordinateModels, includeParameters));

    /// <summary>
    /// Maps DataObjectModel to DataObjectClusteringAnalysisDto with coordinates.
    /// </summary>
    private static DataObjectClusteringAnalysisDto MapDataObjectForClustering(
        DataObjectModel dataObject,
        List<DataObjectCoordinateModel> coordinateModels,
        bool includeParameters)
    {
        var parameterValues = MapParameterValues(dataObject.Values, includeParameters);
        var coordinates = coordinateModels.FirstOrDefault(c => c.ObjectId == dataObject.Id);

        return new DataObjectClusteringAnalysisDto
        {
            Id = dataObject.Id,
            Name = dataObject.Name,
            ParameterValues = parameterValues!,
            X = coordinates?.X ?? 0.0,
            Y = coordinates?.Y ?? 0.0
        };
    }
}
