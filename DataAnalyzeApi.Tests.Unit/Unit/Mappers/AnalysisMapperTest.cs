using DataAnalyzeApi.Mappers;
using DataAnalyzeApi.Tests.Common.Assertions;
using DataAnalyzeApi.Tests.Common.Factories;
using DataAnalyzeApi.Tests.Common.Models.Analyse;

namespace DataAnalyzeApi.Tests.Unit.Mappers;

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
            new TestCluster
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
            new TestCluster
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
            new TestCluster
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
            new TestCluster
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
    public void MapSimilarityPair_ReturnsCorrectSimilarityPairDto()
    {
        // Arrange
        const bool includeParameterValues = false;
        var similarityPair = analysisModelFactory.CreateSimilarityPair();

        // Act
        var result = mapper.MapSimilarityPair(similarityPair, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairEqualDto(
            similarityPair,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapSimilarityPairList_ReturnsCorrectSimilarityPairDtoList()
    {
        // Arrange
        const bool includeParameterValues = false;
        var similarityPairs = analysisModelFactory.CreateSimilarityPairList(3);

        // Act
        var result = mapper.MapSimilarityPairList(similarityPairs, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairListEqualDtoList(
            similarityPairs,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapSimilarityPairList_WithIncludeParameterValues_ReturnsCorrectSimilarityPairDtoList()
    {
        // Arrange
        const bool includeParameterValues = true;
        var similarityPairs = analysisModelFactory.CreateSimilarityPairList(3);

        // Act
        var result = mapper.MapSimilarityPairList(similarityPairs, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairListEqualDtoList(
            similarityPairs,
            result,
            includeParameterValues);
    }
}
