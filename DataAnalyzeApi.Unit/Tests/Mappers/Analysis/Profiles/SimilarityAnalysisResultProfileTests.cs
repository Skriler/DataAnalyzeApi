using AutoMapper;
using DataAnalyzeApi.Mappers.Analysis.Profiles;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;
using DataAnalyzeApi.Unit.Common.Assertions;
using DataAnalyzeApi.Unit.Common.Factories;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis.Profiles;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class SimilarityAnalysisResultProfileTests
{
    private readonly IMapper mapper;
    private readonly AnalysisResultTestFactory factory;

    public SimilarityAnalysisResultProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<SimilarityAnalysisResultProfile>());

        mapper = configuration.CreateMapper();
        factory = new AnalysisResultTestFactory();
    }

    [Fact]
    public void MapSimilarityAnalysisResulto_ReturnsCorrectDto()
    {
        // Arrange
        var result = factory.CreateSimilarityAnalysisResult(pairsCount: 5);

        // Act
        var resultDto = mapper.Map<SimilarityAnalysisResultDto>(result);

        // Assert
        AnalysisResultMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapSimilarityAnalysisResult_WithSinglePair_ReturnsCorrectDto()
    {
        // Arrange
        var result = factory.CreateSimilarityAnalysisResult(pairsCount: 1);

        // Act
        var resultDto = mapper.Map<SimilarityAnalysisResultDto>(result);

        // Assert
        AnalysisResultMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapSimilarityAnalysisResult_WithEmptyPairs_ReturnsCorrectDto()
    {
        // Arrange
        var result = factory.CreateSimilarityAnalysisResult(pairsCount: 0);

        // Act
        var resultDto = mapper.Map<SimilarityAnalysisResultDto>(result);

        // Assert
        AnalysisResultMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapSimilarityAnalysisResultDto_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateSimilarityAnalysisResultDto(pairsCount: 5);

        // Act
        var result = mapper.Map<SimilarityAnalysisResult>(resultDto);

        // Assert
        AnalysisResultMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapSimilarityAnalysisResultDto_WithSinglePair_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateSimilarityAnalysisResultDto(pairsCount: 1);

        // Act
        var result = mapper.Map<SimilarityAnalysisResult>(resultDto);

        // Assert
        AnalysisResultMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapSimilarityAnalysisResultDto_WithEmptyPairs_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateSimilarityAnalysisResultDto(pairsCount: 0);

        // Act
        var result = mapper.Map<SimilarityAnalysisResult>(resultDto);

        // Assert
        AnalysisResultMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto);
    }
}
