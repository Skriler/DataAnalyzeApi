using DataAnalyzeApi.Models.DTOs.Analyse.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings.Similarity.Results;
using DataAnalyzeApi.Models.DTOs.Dataset;

namespace DataAnalyzeApi.Integration.Common.Assertions;

public static class AnalysisResultAssertions
{
    /// <summary>
    /// Verifies that the SimilarityResult is valid and contains expected data.
    /// </summary>
    public static void AssertSimilarityResult(
        SimilarityResult? result,
        long expectedDatasetId,
        bool expectParameterValues)
    {
        Assert.NotNull(result);
        Assert.Equal(expectedDatasetId, result.DatasetId);
        Assert.NotEmpty(result.Similarities);

        Assert.All(
            result.Similarities,
            similarity => AssertSimilarityPair(similarity, expectParameterValues));
    }

    /// <summary>
    /// Verifies that the ClusteringResult is valid and contains expected data.
    /// </summary>
    public static void AssertClusteringResult(
        ClusteringResult? result,
        long expectedDatasetId,
        bool expectParameterValues)
    {
        Assert.NotNull(result);
        Assert.Equal(expectedDatasetId, result.DatasetId);
        Assert.NotEmpty(result.Clusters);

        Assert.All(
            result.Clusters,
            cluster => AssertCluster(cluster, expectParameterValues));
    }

    /// <summary>
    /// Verifies that both objects in the SimilarityPair have expected parameter values,
    /// present and non-empty if expectParameterValues is true,
    /// otherwise, ensures they are null.
    /// </summary>
    private static void AssertSimilarityPair(
        SimilarityPairDto similarity,
        bool expectParameterValues)
    {
        AssertParameterValues(similarity.ObjectA, expectParameterValues);
        AssertParameterValues(similarity.ObjectB, expectParameterValues);
    }

    /// <summary>
    /// Verifies that all objects in the cluster have expected parameter values,
    /// present and non-empty if expectParameterValues is true,
    /// otherwise, ensures they are null.
    /// </summary>
    private static void AssertCluster(ClusterDto cluster, bool expectParameterValues) =>
        Assert.All(cluster.Objects, obj => AssertParameterValues(obj, expectParameterValues));

    /// <summary>
    /// Verifies that the object's parameter values are present or absent as expected.
    /// </summary>
    private static void AssertParameterValues(DataObjectDto? obj, bool expectParameterValues)
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
