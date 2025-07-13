using DataAnalyzeApi.Mappers.Analysis;
using DataAnalyzeApi.Unit.Common.Assertions;
using DataAnalyzeApi.Unit.Common.Factories;
using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Tests.Mappers.Analysis;

[Trait("Category", "Unit")]
[Trait("Component", "Mapper")]
[Trait("SubComponent", "Analysis")]
public class AnalysisMapperTest
{
    private readonly AnalysisMapper mapper = new();
    private readonly AnalysisModelFactory analysisModelFactory = new();

    [Fact]
    public void MapClusterModel_ReturnsCorrectClusterDto()
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

        var cluster = analysisModelFactory.CreateClusterModel(rawCluster);

        // Act
        var result = mapper.MapCluster(cluster, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertClusterModelEqualDto(
            cluster,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapClusterModelList_ReturnsCorrectClusterDtoList()
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

        var clusters = analysisModelFactory.CreateClusterModelList(rawClusters);

        // Act
        var result = mapper.MapClusterList(clusters, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertClusterModelListEqualDtoList(
            clusters,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapClusterModelList_WithIncludeParameterValues_ReturnsCorrectClusterDtoList()
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

        var clusters = analysisModelFactory.CreateClusterModelList(rawClusters);

        // Act
        var result = mapper.MapClusterList(clusters, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertClusterModelListEqualDtoList(
            clusters,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapSimilarityPairModel_ReturnsCorrectSimilarityPairDto()
    {
        // Arrange
        const bool includeParameterValues = false;
        var similarityPair = analysisModelFactory.CreateSimilarityPairModel();

        // Act
        var result = mapper.MapSimilarityPair(similarityPair, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairModelEqualDto(
            similarityPair,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapSimilarityPairModelList_ReturnsCorrectSimilarityPairDtoList()
    {
        // Arrange
        const bool includeParameterValues = false;
        var similarityPairs = analysisModelFactory.CreateSimilarityPairModelList(3);

        // Act
        var result = mapper.MapSimilarityPairList(similarityPairs, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairModelListEqualDtoList(
            similarityPairs,
            result,
            includeParameterValues);
    }

    [Fact]
    public void MapSimilarityPairModelList_WithIncludeParameterValues_ReturnsCorrectSimilarityPairDtoList()
    {
        // Arrange
        const bool includeParameterValues = true;
        var similarityPairs = analysisModelFactory.CreateSimilarityPairModelList(3);

        // Act
        var result = mapper.MapSimilarityPairList(similarityPairs, includeParameterValues);

        // Assert
        AnalysisMapperAssertions.AssertSimilarityPairModelListEqualDtoList(
            similarityPairs,
            result,
            includeParameterValues);
    }
}
