using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Unit.Common.Assertions;
using DataAnalyzeApi.Unit.Common.Factories;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis.Entities;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class SimilarityEntityAnalysisMapperTests
{
    private readonly SimlarityEntityAnalysisMapper mapper = new();
    private readonly EntityAnalysisTestFactory factory = new();

    [Fact]
    public void MapAnalysisResult_WithoutParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = false;
        var result = factory.CreateSimilarityAnalysisResult(pairsCount: 3);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        EntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
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
        EntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
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
        EntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
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
        EntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }
}
