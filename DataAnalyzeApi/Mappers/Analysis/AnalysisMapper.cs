using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

namespace DataAnalyzeApi.Mappers.Analysis;

public class AnalysisMapper
{
    /// <summary>
    /// Maps ClusterModel to its DTO.
    /// </summary>
    public virtual ClusterDto MapCluster(ClusterModel cluster, bool includeParameters = false)
    {
        var objectsDto = cluster.Objects
            .ConvertAll(obj => MapDataObject(obj, includeParameters));

        return new ClusterDto(cluster.Name, objectsDto);
    }

    /// <summary>
    /// Maps ClusterModel list to dto list.
    /// </summary>
    public virtual List<ClusterDto> MapClusterList(List<ClusterModel> clusters, bool includeParameters = false)
        => clusters.ConvertAll(c => MapCluster(c, includeParameters));

    /// <summary>
    /// Maps SimilarityPairModel to its DTO.
    /// </summary>
    public virtual SimilarityPairDto MapSimilarityPair(SimilarityPairModel pair, bool includeParameters = false)
    {
        return new SimilarityPairDto(
            MapDataObject(pair.ObjectA, includeParameters),
            MapDataObject(pair.ObjectB, includeParameters),
            pair.SimilarityPercentage
            );
    }

    /// <summary>
    /// Maps SimilarityPairModel list to their DTOs.
    /// </summary>
    public virtual List<SimilarityPairDto> MapSimilarityPairList(List<SimilarityPairModel> pairs, bool includeParameters = false)
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
