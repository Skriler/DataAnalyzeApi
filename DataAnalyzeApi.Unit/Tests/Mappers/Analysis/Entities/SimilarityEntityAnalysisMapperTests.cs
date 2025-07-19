using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Unit.Common.Assertions.Analysis.Entities;
using DataAnalyzeApi.Unit.Common.Factories.Analysis.Entities;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis.Entities;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class SimilarityEntityAnalysisMapperTests
{
    private readonly SimilarityEntityAnalysisMapper mapper = new();
    private readonly SimilarityEntityAnalysisTestFactory factory = new();

    [Fact]
    public void MapAnalysisResult_WithoutParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = false;
        var result = factory.CreateSimilarityAnalysisResult(pairsCount: 3);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        SimilarityEntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }

    [Fact]
    public void MapAnalysisResult_WithParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = true;
        var result = factory.CreateSimilarityAnalysisResult(pairsCount: 5);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        SimilarityEntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }

    [Fact]
    public void MapAnalysisResult_WithEmptyPairs_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = false;
        var result = factory.CreateSimilarityAnalysisResult(pairsCount: 0);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        SimilarityEntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }

    [Fact]
    public void MapAnalysisResult_WithSinglePair_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = true;
        var result = factory.CreateSimilarityAnalysisResult(pairsCount: 1);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        SimilarityEntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }
}
