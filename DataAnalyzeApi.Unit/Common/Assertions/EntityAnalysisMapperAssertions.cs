using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Unit.Common.Assertions;

public static class EntityAnalysisMapperAssertions
{
    /// <summary>
    /// Verifies that ClusteringAnalysisResult matches the expected ClusteringAnalysisResultDto.
    /// </summary>
    public static void AssertClusteringAnalysisResultEqualDto(
        ClusteringAnalysisResult result,
        ClusteringAnalysisResultDto resultDto,
        bool includeParameterValues = false)
    {
        Assert.Equal(result.DatasetId, resultDto.DatasetId);

        AssertClusterListEqualDtoList(
            result.Clusters,
            resultDto.Clusters,
            includeParameterValues);
    }

    /// <summary>
    /// Verifies that list of Cluster entities matches the expected list of ClusterDto.
    /// </summary>
    public static void AssertClusterListEqualDtoList(
        IList<Cluster> clusters,
        IList<ClusterDto> clusterDtos,
        bool includeParameterValues = false)
    {
        Assert.Equal(clusters.Count, clusterDtos.Count);

        for (int i = 0; i < clusters.Count; ++i)
        {
            AssertClusterEqualDto(
                clusters[i],
                clusterDtos[i],
                includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that Cluster matches the expected ClusterDto.
    /// </summary>
    public static void AssertClusterEqualDto(
        Cluster cluster,
        ClusterDto clusterDto,
        bool includeParameterValues = false)
    {
        Assert.Equal(cluster.Objects.Count, clusterDto.Objects.Count);

        for (int i = 0; i < cluster.Objects.Count; ++i)
        {
            AssertDataObjectEqualDto(
                cluster.Objects[i],
                clusterDto.Objects[i],
                includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that SimilarityAnalysisResult matches the expected SimilarityAnalysisResultDto.
    /// </summary>
    public static void AssertSimilarityAnalysisResultEqualDto(
        SimilarityAnalysisResult result,
        SimilarityAnalysisResultDto resultDto,
        bool includeParameterValues = false)
    {
        Assert.Equal(result.DatasetId, resultDto.DatasetId);

        AssertSimilarityPairListEqualDtoList(
            result.SimilarityPairs,
            resultDto.Similarities,
            includeParameterValues);
    }

    /// <summary>
    /// Verifies that list of SimilarityPair entities matches the expected list of SimilarityPairDto.
    /// </summary>
    public static void AssertSimilarityPairListEqualDtoList(
        IList<SimilarityPair> similarityPairs,
        IList<SimilarityPairDto> similarityPairDtos,
        bool includeParameterValues = false)
    {
        Assert.Equal(similarityPairs.Count, similarityPairDtos.Count);

        for (int i = 0; i < similarityPairs.Count; ++i)
        {
            AssertSimilarityPairEqualDto(
                similarityPairs[i],
                similarityPairDtos[i],
                includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that SimilarityPair matches the expected SimilarityPairDto.
    /// </summary>
    public static void AssertSimilarityPairEqualDto(
        SimilarityPair pair,
        SimilarityPairDto pairDto,
        bool includeParameterValues = false)
    {
        Assert.Equal(pair.SimilarityPercentage, pairDto.SimilarityPercentage);

        AssertDataObjectEqualDto(
            pair.ObjectA,
            pairDto.ObjectA,
            includeParameterValues);

        AssertDataObjectEqualDto(
            pair.ObjectB,
            pairDto.ObjectB,
            includeParameterValues);
    }

    /// <summary>
    /// Verifies that DataObject matches the expected DataObjectAnalysisDto.
    /// </summary>
    private static void AssertDataObjectEqualDto(
        DataObject dataObject,
        DataObjectAnalysisDto dataObjectAnalysisDto,
        bool includeParameterValues = false)
    {
        Assert.Equal(dataObject.Id, dataObjectAnalysisDto.Id);
        Assert.Equal(dataObject.Name, dataObjectAnalysisDto.Name);

        if (includeParameterValues)
        {
            AssertParameterValuesEqualDto(
                dataObject.Values,
                dataObjectAnalysisDto.ParameterValues);
        }
        else
        {
            Assert.Null(dataObjectAnalysisDto.ParameterValues);
        }
    }

    /// <summary>
    /// Verifies that ParameterValue matches the expected ParameterValues dictionary.
    /// </summary>
    private static void AssertParameterValuesEqualDto(
        List<ParameterValue> values,
        Dictionary<string, string> valuesDto)
    {
        Assert.NotNull(valuesDto);
        Assert.Equal(values.Count, valuesDto.Count);

        foreach (var valueModel in values)
        {
            var paramName = valueModel.Parameter.Name;

            Assert.True(valuesDto.ContainsKey(paramName), $"Missing parameter: {paramName}");
            Assert.Equal(valueModel.Value, valuesDto[paramName]);
        }
    }
}
