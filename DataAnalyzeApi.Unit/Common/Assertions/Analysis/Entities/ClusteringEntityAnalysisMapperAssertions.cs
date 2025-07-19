using DataAnalyzeApi.Models.DTOs.Analysis.Clustering;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;

namespace DataAnalyzeApi.Unit.Common.Assertions.Analysis.Entities;

public static class ClusteringEntityAnalysisMapperAssertions
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
        Assert.Equal(result.Algorithm, resultDto.Algorithm);

        var coordinatesDict = result.ObjectCoordinates
            .ToDictionary(coord => coord.ObjectId, coord => coord);

        AssertClusterListEqualDtoList(
            result.Clusters,
            resultDto.Clusters,
            coordinatesDict,
            includeParameterValues);
    }

    /// <summary>
    /// Verifies that list of Cluster entities matches the expected list of ClusterDto.
    /// </summary>
    public static void AssertClusterListEqualDtoList(
        IList<Cluster> clusters,
        IList<ClusterDto> clusterDtos,
        Dictionary<long, DataObjectCoordinate> coordinatesDict,
        bool includeParameterValues = false)
    {
        Assert.Equal(clusters.Count, clusterDtos.Count);

        for (int i = 0; i < clusters.Count; ++i)
        {
            AssertClusterEqualDto(
                clusters[i],
                clusterDtos[i],
                coordinatesDict,
                includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that Cluster matches the expected ClusterDto.
    /// </summary>
    public static void AssertClusterEqualDto(
        Cluster cluster,
        ClusterDto clusterDto,
        Dictionary<long, DataObjectCoordinate> coordinatesDict,
        bool includeParameterValues = false)
    {
        Assert.Equal(cluster.Objects.Count, clusterDto.Objects.Count);

        for (int i = 0; i < cluster.Objects.Count; ++i)
        {
            AssertDataObjectClusteringEqualDto(
                cluster.Objects[i],
                clusterDto.Objects[i],
                coordinatesDict,
                includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that DataObject matches the expected DataObjectClusteringAnalysisDto with coordinates.
    /// </summary>
    public static void AssertDataObjectClusteringEqualDto(
        DataObject dataObject,
        DataObjectClusteringAnalysisDto dataObjectDto,
        Dictionary<long, DataObjectCoordinate> coordinatesDict,
        bool includeParameterValues = false)
    {
        Assert.Equal(dataObject.Id, dataObjectDto.Id);
        Assert.Equal(dataObject.Name, dataObjectDto.Name);

        // Assert coordinates
        coordinatesDict.TryGetValue(dataObject.Id, out var coords);
        var expectedX = coords?.X ?? 0.0;
        var expectedY = coords?.Y ?? 0.0;

        Assert.Equal(expectedX, dataObjectDto.X);
        Assert.Equal(expectedY, dataObjectDto.Y);

        if (includeParameterValues)
        {
            BaseEntityAnalysisMapperAssertions.AssertParameterValuesEqualDto(
                dataObject.Values,
                dataObjectDto.ParameterValues);
        }
        else
        {
            Assert.Null(dataObjectDto.ParameterValues);
        }
    }
}
