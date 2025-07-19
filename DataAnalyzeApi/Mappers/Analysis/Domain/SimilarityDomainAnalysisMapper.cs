using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

namespace DataAnalyzeApi.Mappers.Analysis.Domain;

/// <summary>
/// Mapper for converting similarity domain models to analysis DTOs.
/// </summary>
public class SimilarityDomainAnalysisMapper : BaseDomainAnalysisMapper
{
    /// <summary>
    /// Maps SimilarityPairModel to its DTO.
    /// </summary>
    public virtual SimilarityPairDto Map(
        SimilarityPairModel pair,
        bool includeParameters = false)
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
    public virtual List<SimilarityPairDto> MapList(
        List<SimilarityPairModel> pairs,
        bool includeParameters = false) =>
        pairs.ConvertAll(p => Map(p, includeParameters));
}
