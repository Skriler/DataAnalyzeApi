using DataAnalyzeAPI.Models.Domain.Clustering;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeAPI.Models.DTOs.Dataset;

namespace DataAnalyzeAPI.Mappers;

public class ClusterMapper
{
    /// <summary>
    /// Maps cluster list to their DTOs.
    /// </summary>
    public List<ClusterDto> MapList(List<Cluster> clusters, bool includeParameters)
        => clusters.ConvertAll(c => Map(c, includeParameters));

    /// <summary>
    /// Maps cluster to its DTO.
    /// </summary>
    public ClusterDto Map(Cluster cluster, bool includeParameters)
    {
        var objectsDto = cluster.Objects
            .ConvertAll(obj => MapDataObject(obj, includeParameters));

        return new ClusterDto(cluster.Id, objectsDto);
    }

    /// <summary>
    /// Maps data object to its DTO.
    /// </summary>
    private DataObjectDto MapDataObject(DataObjectModel obj, bool includeParameters)
    {
        var parameterValues = MapParameterValues(obj, includeParameters);

        return new DataObjectDto(obj.Id, obj.Name, parameterValues);
    }

    /// <summary>
    /// Maps parameter values to a dictionary, optionally including the parameters.
    /// </summary>
    private Dictionary<string, string> MapParameterValues(DataObjectModel obj, bool includeParameters)
    {
        if (!includeParameters)
            return new();

        return obj.Values.ToDictionary(
            pv => pv.Parameter.Name,
            pv => pv.Value
        );
    }
}
