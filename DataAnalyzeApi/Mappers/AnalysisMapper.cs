using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

namespace DataAnalyzeApi.Mappers;

public class AnalysisMapper
{
    /// <summary>
    /// Maps Cluster to its DTO.
    /// </summary>
    public virtual ClusterDto MapCluster(Cluster cluster, bool includeParameters = false)
    {
        var objectsDto = cluster.Objects
            .ConvertAll(obj => MapDataObject(obj, includeParameters));

        return new ClusterDto(cluster.Name, objectsDto);
    }

    /// <summary>
    /// Maps Cluster list to their DTOs.
    /// </summary>
    public virtual List<ClusterDto> MapClusterList(List<Cluster> clusters, bool includeParameters = false)
        => clusters.ConvertAll(c => MapCluster(c, includeParameters));

    /// <summary>
    /// Maps SimilarityPair to its DTO.
    /// </summary>
    public virtual SimilarityPairDto MapSimilarityPair(SimilarityPair pair, bool includeParameters = false)
    {
        return new SimilarityPairDto(
            MapDataObject(pair.ObjectA, includeParameters),
            MapDataObject(pair.ObjectB, includeParameters),
            pair.SimilarityPercentage
            );
    }

    /// <summary>
    /// Maps SimilarityPair list to their DTOs.
    /// </summary>
    public virtual List<SimilarityPairDto> MapSimilarityPairList(List<SimilarityPair> pairs, bool includeParameters = false)
        => pairs.ConvertAll(p => MapSimilarityPair(p, includeParameters));

    /// <summary>
    /// Maps DataObjectModel to its DTO.
    /// </summary>
    private static DataObjectAnalysisDto MapDataObject(DataObjectModel dataObject, bool includeParameters)
    {
        var parameterValues = MapParameterValues(dataObject.Values, includeParameters);

        return new DataObjectAnalysisDto(
            dataObject.Id,
            dataObject.Name,
            parameterValues!);
    }

    /// <summary>
    /// Maps ParameterValueModel list to a dictionary (name, value), optionally including the parameters.
    /// Returns null when includeParameters is false, which will cause the property
    /// to be excluded from JSON serialization when used with JsonIgnore attribute.
    /// </summary>
    private static Dictionary<string, string>? MapParameterValues(List<ParameterValueModel> obj, bool includeParameters)
    {
        if (!includeParameters)
            return null;

        return obj.ToDictionary(
            pv => pv.Parameter.Name,
            pv => pv.Value
        );
    }
}
