using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Unit.Common.Assertions;
using DataAnalyzeApi.Unit.Common.Factories;
using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Tests.Mappers;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
public class AnalysisMapperTest
{
    private readonly AnalysisMapper mapper = new();
    private readonly AnalysisModelFactory analysisModelFactory = new();

    [Fact]
    public void MapCluster_ReturnsCorrectClusterDto()
    {
        // Arrange
        const bool includeParameterValues = false;
        var rawCluster = new TestCluster()
        {
            Objects =
            [
                new NormalizedDataObject
                {
                    NumericValues = [0.5, 0.7],
                    CategoricalValues = [[0, 1]],
                },
                new NormalizedDataObject
                {
                    NumericValues = [0.6, 0.8],
                    CategoricalValues = [[1, 1]],
                },
            ]
        };

        var cluster = analysisModelFactory.CreateCluster(rawCluster);

        // Act
        var result = mapper.MapCluster(cluster, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertClusterEqualDto(
            cluster,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapClusterList_ReturnsCorrectClusterDtoList()
    {
        // Arrange
        const bool includeParameterValues = false;
        var rawClusters = new List<TestCluster>()
        {
            new()
            {
                Objects =
                [
                    new NormalizedDataObject
                    {
                        NumericValues = [0.5, 0.7],
                        CategoricalValues = [[0, 1]],
                    },
                    new NormalizedDataObject
                    {
                        NumericValues = [0.6, 0.8],
                        CategoricalValues = [[1, 1]],
                    },
                ]
            },
            new()
            {
                Objects =
                [
                    new NormalizedDataObject
                    {
                        NumericValues = [0.1, 0.2],
                        CategoricalValues = [[1, 1]],
                    },
                    new NormalizedDataObject
                    {
                        NumericValues = [0.15, 0.3],
                        CategoricalValues = [[0, 1]],
                    },
                ]
            },
        };

        var clusters = analysisModelFactory.CreateClusterList(rawClusters);

        // Act
        var result = mapper.MapClusterList(clusters, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertClusterListsEqualDtoList(
            clusters,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapClusterList_WithIncludeParameterValues_ReturnsCorrectClusterDtoList()
    {
        // Arrange
        const bool includeParameterValues = true;
        var rawClusters = new List<TestCluster>()
        {
            new()
            {
                Objects =
                [
                    new NormalizedDataObject
                    {
                        NumericValues = [0.12, 0.1],
                        CategoricalValues = [[1, 1]],
                    },
                    new NormalizedDataObject
                    {
                        NumericValues = [0.05, 0.14],
                        CategoricalValues = [[1, 0]],
                    },
                ]
            },
            new()
            {
                Objects =
                [
                    new NormalizedDataObject
                    {
                        NumericValues = [0.7, 0.4],
                        CategoricalValues = [[1, 1, 0]],
                    },
                    new NormalizedDataObject
                    {
                        NumericValues = [0.55, 0.6],
                        CategoricalValues = [[0, 1, 1]],
                    },
                ]
            },
        };

        var clusters = analysisModelFactory.CreateClusterList(rawClusters);

        // Act
        var result = mapper.MapClusterList(clusters, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertClusterListsEqualDtoList(
            clusters,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapSimilarityPairDto_ReturnsCorrectSimilarityPairDtoDto()
    {
        // Arrange
        const bool includeParameterValues = false;
        var SimilarityPairDto = analysisModelFactory.CreateSimilarityPairDto();

        // Act
        var result = mapper.MapSimilarityPairDto(SimilarityPairDto, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairDtoEqualDto(
            SimilarityPairDto,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapSimilarityPairDtoList_ReturnsCorrectSimilarityPairDtoDtoList()
    {
        // Arrange
        const bool includeParameterValues = false;
        var SimilarityPairDtos = analysisModelFactory.CreateSimilarityPairDtoList(3);

        // Act
        var result = mapper.MapSimilarityPairDtoList(SimilarityPairDtos, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairDtoListEqualDtoList(
            SimilarityPairDtos,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapSimilarityPairDtoList_WithIncludeParameterValues_ReturnsCorrectSimilarityPairDtoDtoList()
    {
        // Arrange
        const bool includeParameterValues = true;
        var SimilarityPairDtos = analysisModelFactory.CreateSimilarityPairDtoList(3);

        // Act
        var result = mapper.MapSimilarityPairDtoList(SimilarityPairDtos, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairDtoListEqualDtoList(
            SimilarityPairDtos,
            result,
            includeParameterValues);
    }
}
