using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

namespace DataAnalyzeApi.Unit.Common.Assertions.Analysis.Domain;

public static class SimilarityDomainAnalysisMapperAssertions
{
    /// <summary>
    /// Verifies that a SimilarityPairModel matches the expected SimilarityPairDto.
    /// </summary>
    public static void AssertSimilarityPairModelEqualDto(
        SimilarityPairModel expected,
        SimilarityPairDto actual,
        bool includeParameterValues)
    {
        Assert.Equal(expected.SimilarityPercentage, actual.SimilarityPercentage);

        BaseDomainAnalysisMapperAssertions.AssertDataObjectsEqualDto(
            expected.ObjectA,
            actual.ObjectA,
            includeParameterValues);

        BaseDomainAnalysisMapperAssertions.AssertDataObjectsEqualDto(
            expected.ObjectB,
            actual.ObjectB,
            includeParameterValues);
    }

    /// <summary>
    /// Verifies that SimilarityPairModel list matches the expected SimilarityPairDto list.
    /// </summary>
    public static void AssertSimilarityPairModelListEqualDtoList(
        List<SimilarityPairModel> expectedList,
        List<SimilarityPairDto> actualList,
        bool includeParameterValues)
    {
        Assert.Equal(expectedList.Count, actualList.Count);

        for (int i = 0; i < expectedList.Count; ++i)
        {
            AssertSimilarityPairModelEqualDto(
                expectedList[i],
                actualList[i],
                includeParameterValues);
        }
    }
}
