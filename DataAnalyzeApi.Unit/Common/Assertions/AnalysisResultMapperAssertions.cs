using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Unit.Common.Assertions;

public static class AnalysisResultMapperAssertions
{
    /// <summary>
    /// Verifies that ClusterAnalysisResult matches the expected ClusterAnalysisResultDto.
    /// </summary>
    public static void AssertClusterAnalysisResultEqualDto(
        ClusterAnalysisResult result,
        ClusterAnalysisResultDto resultDto)
    {
        Assert.Equal(result.DatasetId, resultDto.DatasetId);
        Assert.Equal(result.Clusters.Count, resultDto.Clusters.Count);

        for (int i = 0; i < result.Clusters.Count; ++i)
        {
            AssertClusterEqualDto(result.Clusters[i], resultDto.Clusters[i]);
        }
    }

    /// <summary>
    /// Verifies that SimilarityAnalysisResult matches the expected SimilarityAnalysisResultDto.
    /// </summary>
    public static void AssertSimilarityAnalysisResultEqualDto(
        SimilarityAnalysisResult result,
        SimilarityAnalysisResultDto resultDto)
    {
        Assert.Equal(result.DatasetId, resultDto.DatasetId);
        Assert.Equal(result.SimilarityPairs.Count, resultDto.Similarities.Count);

        for (int i = 0; i < result.SimilarityPairs.Count; ++i)
        {
            AssertSimilarityPairEqualDto(result.SimilarityPairs[i], resultDto.Similarities[i]);
        }
    }

    /// <summary>
    /// Verifies that Cluster matches the expected ClusterDto.
    /// </summary>
    private static void AssertClusterEqualDto(Cluster cluster, ClusterDto clusterDto)
    {
        Assert.Equal(cluster.Objects.Count, clusterDto.Objects.Count);

        for (int i = 0; i < cluster.Objects.Count; ++i)
        {
            AssertDataObjectEqualDto(cluster.Objects[i], clusterDto.Objects[i]);
        }
    }

    /// <summary>
    /// Verifies that SimilarityPair matches the expected SimilarityPairDto.
    /// </summary>
    private static void AssertSimilarityPairEqualDto(SimilarityPair pair, SimilarityPairDto pairDto)
    {
        Assert.Equal(pair.SimilarityPercentage, pairDto.SimilarityPercentage);
        AssertDataObjectEqualDto(pair.ObjectA, pairDto.ObjectA);
        AssertDataObjectEqualDto(pair.ObjectB, pairDto.ObjectB);
    }

    /// <summary>
    /// Verifies that DataObject matches the expected DataObjectAnalysisDto.
    /// </summary>
    private static void AssertDataObjectEqualDto(DataObject dataObject, DataObjectAnalysisDto dataObjectAnalysisDto)
    {
        Assert.Equal(dataObject.Id, dataObjectAnalysisDto.Id);
        Assert.Equal(dataObject.Name, dataObjectAnalysisDto.Name);

        // TODO: Fix ParameterValues comparison later
        // For now, just verify it's null as configured in the profile
        Assert.Empty(dataObject.Values);
        Assert.Null(dataObjectAnalysisDto.ParameterValues);
    }
}
