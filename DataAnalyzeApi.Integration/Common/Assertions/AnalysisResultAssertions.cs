using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.DTOs.Analysis;

namespace DataAnalyzeApi.Integration.Common.Assertions;

public static class AnalysisResultAssertions
{
    /// <summary>
    /// Verifies that the SimilarityResult is valid and contains expected data.
    /// </summary>
    public static void AssertSimilarityResult(
        SimilarityAnalysisResultDto? result,
        long expectedDatasetId,
        bool expectParameterValues)
    {
        Assert.NotNull(result);
        Assert.Equal(expectedDatasetId, result.DatasetId);
        Assert.NotEmpty(result.Similarities);

        Assert.All(
            result.Similarities,
            similarity => AssertSimilarityPairDto(similarity, expectParameterValues));
    }

    /// <summary>
    /// Verifies that the ClusteringResult is valid and contains expected data.
    /// </summary>
    public static void AssertClusteringResult(
        ClusterAnalysisResultDto? result,
        long expectedDatasetId,
        bool expectParameterValues)
    {
        Assert.NotNull(result);
        Assert.Equal(expectedDatasetId, result.DatasetId);
        Assert.NotEmpty(result.Clusters);

        Assert.All(
            result.Clusters,
            cluster => AssertClusterDto(cluster, expectParameterValues));
    }

    /// <summary>
    /// Verifies that both objects in the SimilarityPairDto have expected parameter values,
    /// present and non-empty if expectParameterValues is true,
    /// otherwise, ensures they are null.
    /// </summary>
    private static void AssertSimilarityPairDto(
        SimilarityPairDto similarity,
        bool expectParameterValues)
    {
        AssertParameterValues(similarity.ObjectA, expectParameterValues);
        AssertParameterValues(similarity.ObjectB, expectParameterValues);
    }

    /// <summary>
    /// Verifies that all objects in the ClusterDto have expected parameter values,
    /// present and non-empty if expectParameterValues is true,
    /// otherwise, ensures they are null.
    /// </summary>
    private static void AssertClusterDto(ClusterDto cluster, bool expectParameterValues) =>
        Assert.All(cluster.Objects, obj => AssertParameterValues(obj, expectParameterValues));

    /// <summary>
    /// Verifies that the object's parameter values are present or absent as expected.
    /// </summary>
    private static void AssertParameterValues(DataObjectAnalysisDto? obj, bool expectParameterValues)
    {
        Assert.NotNull(obj);

        if (expectParameterValues)
        {
            Assert.NotNull(obj.ParameterValues);
            Assert.NotEmpty(obj.ParameterValues);
        }
        else
        {
            Assert.Null(obj.ParameterValues);
        }
    }
}
