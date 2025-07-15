using AutoMapper;
using DataAnalyzeApi.Mappers.Analysis.Profiles;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Unit.Common.Assertions;
using DataAnalyzeApi.Unit.Common.Factories;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis.Profiles;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class ClusteringAnalysisResultProfileTests
{
    private readonly IMapper mapper;
    private readonly EntityAnalysisTestFactory factory;

    public ClusteringAnalysisResultProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ClusteringAnalysisResultProfile>());

        mapper = configuration.CreateMapper();
        factory = new EntityAnalysisTestFactory();
    }

    [Fact]
    public void MapClusteringAnalysisResultDto_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateClusteringAnalysisResultDto(clustersCount: 3, objectsPerCluster: 5);

        // Act
        var result = mapper.Map<ClusteringAnalysisResult>(resultDto);

        // Assert
        EntityAnalysisMapperAssertions.AssertClusteringAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapClusteringAnalysisResultDto_WithSingleClusters_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateClusteringAnalysisResultDto(clustersCount: 1, objectsPerCluster: 4);

        // Act
        var result = mapper.Map<ClusteringAnalysisResult>(resultDto);

        // Assert
        EntityAnalysisMapperAssertions.AssertClusteringAnalysisResultEqualDto(
            result,
            resultDto);
    }

    [Fact]
    public void MapClusteringAnalysisResultDto_WithEmptyClusters_ReturnsCorrectEntity()
    {
        // Arrange
        var resultDto = factory.CreateClusteringAnalysisResultDto(clustersCount: 0, objectsPerCluster: 0);

        // Act
        var result = mapper.Map<ClusteringAnalysisResult>(resultDto);

        // Assert
        EntityAnalysisMapperAssertions.AssertClusteringAnalysisResultEqualDto(
            result,
            resultDto);
    }
}
