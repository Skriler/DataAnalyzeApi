using DataAnalyzeApi.Mappers.Analysis.Domain;
using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Unit.Common.Assertions.Analysis.Domain;
using DataAnalyzeApi.Unit.Common.Factories.Analysis.Domain;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis.Domain;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class ClusteringDomainAnalysisMapperTests
{
    private readonly ClusteringDomainAnalysisMapper mapper = new();
    private readonly ClusteringDomainAnalysisTestFactory factory = new();

    [Fact]
    public void Map_WithoutParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameterValues = false;
        var cluster = factory.CreateClusterModel(objectsCount: 3);
        var coordinates = factory.CreateDataObjectCoordinateModelListForClusters([cluster]);

        // Act
        var result = mapper.Map(cluster, coordinates, includeParameterValues);

        // Assert
        ClusteringDomainAnalysisMapperAssertions.AssertClusterModelEqualDto(
            cluster,
            result,
            coordinates,
            includeParameterValues);
    }

    [Fact]
    public void Map_WithParameters_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameterValues = true;
        var cluster = factory.CreateClusterModel(objectsCount: 2);
        var coordinates = factory.CreateDataObjectCoordinateModelListForClusters([cluster]);

        // Act
        var result = mapper.Map(cluster, coordinates, includeParameterValues);

        // Assert
        ClusteringDomainAnalysisMapperAssertions.AssertClusterModelEqualDto(
            cluster,
            result,
            coordinates,
            includeParameterValues);
    }

    [Fact]
    public void Map_WithEmptyObjects_ReturnsCorrectDto()
    {
        // Arrange
        const bool includeParameterValues = false;
        var cluster = factory.CreateClusterModel(objectsCount: 0);
        var coordinates = factory.CreateDataObjectCoordinateModelListForClusters([cluster]);

        // Act
        var result = mapper.Map(cluster, coordinates, includeParameterValues);

        // Assert
        ClusteringDomainAnalysisMapperAssertions.AssertClusterModelEqualDto(
            cluster,
            result,
            coordinates,
            includeParameterValues);
    }

    [Fact]
    public void MapList_WithoutParameters_ReturnsCorrectDtoList()
    {
        // Arrange
        const bool includeParameterValues = false;
        var clusters = factory.CreateClusterModelList(clustersCount: 3, objectsPerCluster: 2);
        var coordinates = factory.CreateDataObjectCoordinateModelListForClusters(clusters);

        // Act
        var result = mapper.MapList(clusters, coordinates, includeParameterValues);

        // Assert
        ClusteringDomainAnalysisMapperAssertions.AssertClusterModelListEqualDtoList(
            clusters,
            result,
            coordinates,
            includeParameterValues);
    }

    [Fact]
    public void MapList_WithParameters_ReturnsCorrectDtoList()
    {
        // Arrange
        const bool includeParameterValues = true;
        var clusters = factory.CreateClusterModelList(clustersCount: 2, objectsPerCluster: 4);
        var coordinates = factory.CreateDataObjectCoordinateModelListForClusters(clusters);

        // Act
        var result = mapper.MapList(clusters, coordinates, includeParameterValues);

        // Assert
        ClusteringDomainAnalysisMapperAssertions.AssertClusterModelListEqualDtoList(
            clusters,
            result,
            coordinates,
            includeParameterValues);
    }

    [Fact]
    public void MapList_WithEmptyClusters_ReturnsCorrectDtoList()
    {
        // Arrange
        const bool includeParameterValues = false;
        var clusters = new List<ClusterModel>();
        var coordinates = factory.CreateDataObjectCoordinateModelListForClusters(clusters);

        // Act
        var result = mapper.MapList(clusters, coordinates, includeParameterValues);

        // Assert
        ClusteringDomainAnalysisMapperAssertions.AssertClusterModelListEqualDtoList(
            clusters,
            result,
            coordinates,
            includeParameterValues);
    }

    [Fact]
    public void MapList_WithSingleCluster_ReturnsCorrectDtoList()
    {
        // Arrange
        const bool includeParameterValues = true;
        var clusters = factory.CreateClusterModelList(clustersCount: 1, objectsPerCluster: 5);
        var coordinates = factory.CreateDataObjectCoordinateModelListForClusters(clusters);

        // Act
        var result = mapper.MapList(clusters, coordinates, includeParameterValues);

        // Assert
        ClusteringDomainAnalysisMapperAssertions.AssertClusterModelListEqualDtoList(
            clusters,
            result,
            coordinates,
            includeParameterValues);
    }
}
