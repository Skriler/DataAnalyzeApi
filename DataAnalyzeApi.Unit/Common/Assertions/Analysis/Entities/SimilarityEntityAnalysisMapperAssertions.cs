using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Unit.Common.Assertions.Analysis.Entities;

public static class SimilarityEntityAnalysisMapperAssertions
{
    /// <summary>
    /// Verifies that SimilarityAnalysisResult matches the expected SimilarityAnalysisResultDto.
    /// </summary>
    public static void AssertSimilarityAnalysisResultEqualDto(
        SimilarityAnalysisResult result,
        SimilarityAnalysisResultDto resultDto,
        bool includeParameterValues = false)
    {
        Assert.Equal(result.DatasetId, resultDto.DatasetId);

        AssertSimilarityPairListEqualDtoList(
            result.SimilarityPairs,
            resultDto.Similarities,
            includeParameterValues);
    }

    /// <summary>
    /// Verifies that list of SimilarityPair entities matches the expected list of SimilarityPairDto.
    /// </summary>
    public static void AssertSimilarityPairListEqualDtoList(
        IList<SimilarityPair> similarityPairs,
        IList<SimilarityPairDto> similarityPairDtos,
        bool includeParameterValues = false)
    {
        Assert.Equal(similarityPairs.Count, similarityPairDtos.Count);

        for (int i = 0; i < similarityPairs.Count; ++i)
        {
            AssertSimilarityPairEqualDto(
                similarityPairs[i],
                similarityPairDtos[i],
                includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that SimilarityPair matches the expected SimilarityPairDto.
    /// </summary>
    public static void AssertSimilarityPairEqualDto(
        SimilarityPair pair,
        SimilarityPairDto pairDto,
        bool includeParameterValues = false)
    {
        Assert.Equal(pair.SimilarityPercentage, pairDto.SimilarityPercentage);

        BaseEntityAnalysisMapperAssertions.AssertDataObjectEqualDto(
            pair.ObjectA,
            pairDto.ObjectA,
            includeParameterValues);

        BaseEntityAnalysisMapperAssertions.AssertDataObjectEqualDto(
            pair.ObjectB,
            pairDto.ObjectB,
            includeParameterValues);
    }
}
