using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Unit.Common.Assertions.Analysis.Entities;
using DataAnalyzeApi.Unit.Common.Factories.Analysis.Entities;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis.Entities;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class ClusteringEntityAnalysisMapperTests
{
    private readonly ClusteringEntityAnalysisMapper mapper = new();
    private readonly ClusteringEntityAnalysisTestFactory factory = new();

    [Fact]
    public void MapAnalysisResult_WithoutParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = false;
        var result = factory.CreateClusteringAnalysisResult(
            clustersCount: 3,
            objectsPerCluster: 2);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        ClusteringEntityAnalysisMapperAssertions.AssertClusteringAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }

    [Fact]
    public void MapAnalysisResult_WithParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = true;
        var result = factory.CreateClusteringAnalysisResult(
            clustersCount: 2,
            objectsPerCluster: 4);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        ClusteringEntityAnalysisMapperAssertions.AssertClusteringAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }

    [Fact]
    public void MapAnalysisResult_WithEmptyClusters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = false;
        var result = factory.CreateClusteringAnalysisResult(
            clustersCount: 0,
            objectsPerCluster: 0);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        ClusteringEntityAnalysisMapperAssertions.AssertClusteringAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }

    [Fact]
    public void MapAnalysisResult_WithSingleCluster_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameters = true;
        var result = factory.CreateClusteringAnalysisResult(
            clustersCount: 1,
            objectsPerCluster: 5);

        // Act
        var resultDto = mapper.MapAnalysisResult(result, includeParameters);

        // Assert
        ClusteringEntityAnalysisMapperAssertions.AssertClusteringAnalysisResultEqualDto(
            result,
            resultDto,
            includeParameters);
    }
}
