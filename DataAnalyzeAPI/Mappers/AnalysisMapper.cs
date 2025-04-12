using DataAnalyzeAPI.Models.Domain.Clustering;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Similarity;
using DataAnalyzeAPI.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity.Results;
using DataAnalyzeAPI.Models.DTOs.Dataset;

namespace DataAnalyzeAPI.Mappers;

public class AnalysisMapper
{
    /// <summary>
    /// Maps cluster list to their DTOs.
    /// </summary>
    public List<ClusterDto> MapClusterList(List<Cluster> clusters, bool includeParameters)
        => clusters.ConvertAll(c => MapCluster(c, includeParameters));

    /// <summary>
    /// Maps cluster to its DTO.
    /// </summary>
    public ClusterDto MapCluster(Cluster cluster, bool includeParameters)
    {
        var objectsDto = cluster.Objects
            .ConvertAll(obj => MapDataObject(obj, includeParameters));

        return new ClusterDto(cluster.Name, objectsDto);
    }

    /// <summary>
    /// Maps similarity pair list to their DTOs.
    /// </summary>
    public List<SimilarityPairDto> MapSimilarityPairList(List<SimilarityPair> pairs, bool includeParameters = false)
        => pairs.ConvertAll(p => MapSimilarityPair(p, includeParameters));

    /// <summary>
    /// Maps similarity pair to its DTO.
    /// </summary>
    public SimilarityPairDto MapSimilarityPair(SimilarityPair pair, bool includeParameters = false)
    {
        return new SimilarityPairDto(
            MapDataObject(pair.ObjectA, includeParameters),
            MapDataObject(pair.ObjectB, includeParameters),
            pair.SimilarityPercentage
            );
    }

    /// <summary>
    /// Maps data object to its DTO.
    /// </summary>
    private static DataObjectDto MapDataObject(DataObjectModel obj, bool includeParameters)
    {
        var parameterValues = MapParameterValues(obj, includeParameters);

        return new DataObjectDto(obj.Id, obj.Name, parameterValues!);
    }

    /// <summary>
    /// Maps parameter values to a dictionary, optionally including the parameters.
    /// Returns null when includeParameters is false, which will cause the property
    /// to be excluded from JSON serialization when used with JsonIgnore attribute.
    /// </summary>
    private static Dictionary<string, string>? MapParameterValues(DataObjectModel obj, bool includeParameters)
    {
        if (!includeParameters)
            return null;

        return obj.Values.ToDictionary(
            pv => pv.Parameter.Name,
            pv => pv.Value
        );
    }
}
