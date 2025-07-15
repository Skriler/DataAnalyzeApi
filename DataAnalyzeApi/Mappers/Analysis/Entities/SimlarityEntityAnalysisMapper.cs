
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Mappers.Analysis.Entities;

public class SimlarityEntityAnalysisMapper : BaseEntityAnalysisMapper<SimilarityAnalysisResult, SimilarityAnalysisResultDto>
{
    /// <summary>
    /// Maps analysis result SimilarityAnalysisResult to SimilarityAnalysisResultDto.
    /// </summary>
    public override SimilarityAnalysisResultDto MapAnalysisResult(
        SimilarityAnalysisResult result,
        bool includeParameters = false)
    {
        return new SimilarityAnalysisResultDto
        {
            DatasetId = result.DatasetId,
            Similarities = MapSimilarityPairList(result.SimilarityPairs, includeParameters),
        };
    }

    /// <summary>
    /// Maps SimilarityPair entity list to SimilarityPairDto list.
    /// </summary>
    public virtual List<SimilarityPairDto> MapSimilarityPairList(
        List<SimilarityPair> pairs,
        bool includeParameters = false)
        => pairs.ConvertAll(p => MapSimilarityPair(p, includeParameters));

    /// <summary>
    /// Maps SimilarityPair entity to SimilarityPairDto.
    /// </summary>
    public virtual SimilarityPairDto MapSimilarityPair(
        SimilarityPair pair,
        bool includeParameters = false)
    {
        return new SimilarityPairDto(
            MapDataObject(pair.ObjectA, includeParameters),
            MapDataObject(pair.ObjectB, includeParameters),
            pair.SimilarityPercentage
        );
    }
}
