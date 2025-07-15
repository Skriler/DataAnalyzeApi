using AutoMapper;
using DataAnalyzeApi.Mappers.Analysis.Profiles;
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
    private readonly EntityAnalysisTestFactory factory;

    public SimilarityAnalysisResultProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<SimilarityAnalysisResultProfile>());

        mapper = configuration.CreateMapper();
        factory = new EntityAnalysisTestFactory();
    }

    [Fact]
    public void MapSimilarityAnalysisResultDto_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateSimilarityAnalysisResultDto(pairsCount: 5);

        // Act
        var result = mapper.Map<SimilarityAnalysisResult>(resultDto);

        // Assert
        EntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
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
        EntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
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
        EntityAnalysisMapperAssertions.AssertSimilarityAnalysisResultEqualDto(
            result,
            resultDto);
    }
}
