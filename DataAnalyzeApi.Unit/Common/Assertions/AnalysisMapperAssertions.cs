using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

namespace DataAnalyzeApi.Unit.Common.Assertions;

public static class AnalysisMapperAssertions
{
    /// <summary>
    /// Verifies that the Cluster matches the expected ClusterDto.
    /// </summary>
    public static void AssertClusterEqualDto(
        Cluster cluster,
        ClusterDto result,
        bool includeParameterValues)
    {
        Assert.Equal(cluster.Name, result.Name);
        Assert.Equal(cluster.Objects.Count, result.Objects.Count);

        for (int i = 0; i < cluster.Objects.Count; ++i)
        {
            AssertDataObjectsEqual(cluster.Objects[i], result.Objects[i], includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that Cluster list matches the expected ClusterDto list.
    /// </summary>
    public static void AssertClusterListsEqualDtoList(
        List<Cluster> clusters,
        List<ClusterDto> result,
        bool includeParameterValues)
    {
        Assert.Equal(clusters.Count, result.Count);

        for (int i = 0; i < clusters.Count; ++i)
        {
            AssertClusterEqualDto(clusters[i], result[i], includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that a SimilarityPairDto matches the expected SimilarityPairDtoDto.
    /// </summary>
    public static void AssertSimilarityPairDtoEqualDto(
        SimilarityPairDto expected,
        SimilarityPairDtoDto actual,
        bool includeParameterValues)
    {
        Assert.Equal(expected.SimilarityPercentage, actual.SimilarityPercentage);
        AssertDataObjectsEqual(expected.ObjectA, actual.ObjectA, includeParameterValues);
        AssertDataObjectsEqual(expected.ObjectB, actual.ObjectB, includeParameterValues);
    }

    /// <summary>
    /// Verifies that SimilarityPairDto list matches the expected SimilarityPairDtoDto list.
    /// </summary>
    public static void AssertSimilarityPairDtoListEqualDtoList(
        List<SimilarityPairDto> expectedList,
        List<SimilarityPairDtoDto> actualList,
        bool includeParameterValues)
    {
        Assert.Equal(expectedList.Count, actualList.Count);

        for (int i = 0; i < expectedList.Count; ++i)
        {
            AssertSimilarityPairDtoEqualDto(expectedList[i], actualList[i], includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that DataObjectModel matches the expected DataObjectDto.
    /// </summary>
    private static void AssertDataObjectsEqual(
        DataObjectModel dataObject,
        DataObjectAnalysisDto result,
        bool includeParameterValues)
    {
        Assert.Equal(dataObject.Id, result.Id);
        Assert.Equal(dataObject.Name, result.Name);

        if (includeParameterValues)
        {
            AssertParameterValuesEqual(dataObject.Values, result.ParameterValues);
        }
        else
        {
            Assert.Null(result.ParameterValues);
        }
    }

    /// <summary>
    /// Verifies that ParameterValueModel matches the expected ParameterValues dictionary.
    /// </summary>
    private static void AssertParameterValuesEqual(
        List<ParameterValueModel> valueModels,
        Dictionary<string, string> resultValues)
    {
        Assert.NotNull(resultValues);
        Assert.Equal(valueModels.Count, resultValues.Count);

        foreach (var valueModel in valueModels)
        {
            var paramName = valueModel.Parameter.Name;

            Assert.True(resultValues.ContainsKey(paramName), $"Missing parameter: {paramName}");
            Assert.Equal(valueModel.Value, resultValues[paramName]);
        }
    }
}
