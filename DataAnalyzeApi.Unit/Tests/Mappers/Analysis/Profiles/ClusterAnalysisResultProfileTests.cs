using AutoMapper;
using DataAnalyzeApi.Mappers.Analysis.Profiles;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Unit.Common.Assertions;
using DataAnalyzeApi.Unit.Common.Factories;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis.Profiles;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class ClusterAnalysisResultProfileTests
{
    private readonly IMapper mapper;
    private readonly AnalysisResultTestFactory factory;

    public ClusterAnalysisResultProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ClusterAnalysisResultProfile>());

        mapper = configuration.CreateMapper();
        factory = new AnalysisResultTestFactory();
    }

    [Fact]
    public void MapClusterAnalysisResult_ReturnsCorrectDto()
    {
        // Arrange
        var result = factory.CreateClusterAnalysisResult(clustersCount: 4, objectsPerCluster: 3);

        // Act
        var resultDto = mapper.Map<ClusterAnalysisResultDto>(result);

        // Assert
        AnalysisResultMapperAssertions.AssertClusterAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapClusterAnalysisResult_WithSingleCluster_ReturnsCorrectDto()
    {
        // Arrange
        var result = factory.CreateClusterAnalysisResult(clustersCount: 1, objectsPerCluster: 5);

        // Act
        var resultDto = mapper.Map<ClusterAnalysisResultDto>(result);

        // Assert
        AnalysisResultMapperAssertions.AssertClusterAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapClusterAnalysisResult_WithEmptyClusters_ReturnsCorrectDto()
    {
        // Arrange
        var result = factory.CreateClusterAnalysisResult(clustersCount: 0, objectsPerCluster: 0);

        // Act
        var resultDto = mapper.Map<ClusterAnalysisResultDto>(result);

        // Assert
        AnalysisResultMapperAssertions.AssertClusterAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapClusterAnalysisResultDto_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateClusterAnalysisResultDto(clustersCount: 3, objectsPerCluster: 5);

        // Act
        var result = mapper.Map<ClusterAnalysisResult>(resultDto);

        // Assert
        AnalysisResultMapperAssertions.AssertClusterAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapClusterAnalysisResultDto_WithSingleClusters_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateClusterAnalysisResultDto(clustersCount: 1, objectsPerCluster: 4);

        // Act
        var result = mapper.Map<ClusterAnalysisResult>(resultDto);

        // Assert
        AnalysisResultMapperAssertions.AssertClusterAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapClusterAnalysisResultDto_WithEmptyClusters_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateClusterAnalysisResultDto(clustersCount: 0, objectsPerCluster: 0);

        // Act
        var result = mapper.Map<ClusterAnalysisResult>(resultDto);

        // Assert
        AnalysisResultMapperAssertions.AssertClusterAnalysisResultEqualDto(
            result,
            resultDto);
    }
}
