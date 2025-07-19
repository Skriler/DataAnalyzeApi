using DataAnalyzeApi.Mappers.Analysis.Domain;
using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Unit.Common.Assertions.Analysis.Domain;
using DataAnalyzeApi.Unit.Common.Factories.Analysis.Domain;
namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis.Domain;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class SimilarityDomainAnalysisMapperTests
{
    private readonly SimilarityDomainAnalysisMapper mapper = new();
    private readonly SimilarityDomainAnalysisTestFactory factory = new();

    [Fact]
    public void Map_WithoutParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameterValues = false;
        var similarityPair = factory.CreateSimilarityPairModel();

        // Act
        var result = mapper.Map(similarityPair, includeParameterValues);

        // Assert
        SimilarityDomainAnalysisMapperAssertions.AssertSimilarityPairModelEqualDto(
            similarityPair,
            result,
            includeParameterValues);
    }

    [Fact]
    public void Map_WithParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameterValues = true;
        var similarityPair = factory.CreateSimilarityPairModel();

        // Act
        var result = mapper.Map(similarityPair, includeParameterValues);

        // Assert
        SimilarityDomainAnalysisMapperAssertions.AssertSimilarityPairModelEqualDto(
            similarityPair,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapList_WithoutParameters_ReturnsCorrectDtoList()
    {
        // Arrange
        const bool includeParameterValues = false;
        var similarityPairs = factory.CreateSimilarityPairModelList(3);

        // Act
        var result = mapper.MapList(similarityPairs, includeParameterValues);

        // Assert
        SimilarityDomainAnalysisMapperAssertions.AssertSimilarityPairModelListEqualDtoList(
            similarityPairs,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapList_WithParameters_ReturnsCorrectDtoList()
    {
        // Arrange
        const bool includeParameterValues = true;
        var similarityPairs = factory.CreateSimilarityPairModelList(5);

        // Act
        var result = mapper.MapList(similarityPairs, includeParameterValues);

        // Assert
        SimilarityDomainAnalysisMapperAssertions.AssertSimilarityPairModelListEqualDtoList(
            similarityPairs,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapList_WithEmptyList_ReturnsCorrectDtoList()
    {
        // Arrange
        const bool includeParameterValues = false;
        var similarityPairs = new List<SimilarityPairModel>();

        // Act
        var result = mapper.MapList(similarityPairs, includeParameterValues);

        // Assert
        SimilarityDomainAnalysisMapperAssertions.AssertSimilarityPairModelListEqualDtoList(
            similarityPairs,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapList_WithSinglePair_ReturnsCorrectDtoList()
    {
        // Arrange
        const bool includeParameterValues = true;
        var similarityPairs = factory.CreateSimilarityPairModelList(1);

        // Act
        var result = mapper.MapList(similarityPairs, includeParameterValues);

        // Assert
        SimilarityDomainAnalysisMapperAssertions.AssertSimilarityPairModelListEqualDtoList(
            similarityPairs,
            result,
            includeParameterValues);
    }
}
