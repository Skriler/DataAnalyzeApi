using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.DimensionalityReduction;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;

namespace DataAnalyzeApi.Unit.Common.Assertions.Analysis.Domain;

public static class ClusteringDomainAnalysisMapperAssertions
{
    /// <summary>
    /// Verifies that the ClusterModel matches the expected ClusterDto.
    /// </summary>
    public static void AssertClusterModelEqualDto(
        ClusterModel cluster,
        ClusterDto result,
        List<DataObjectCoordinateModel> coordinates,
        bool includeParameterValues)
    {
        Assert.Equal(cluster.Name, result.Name);
        Assert.Equal(cluster.Objects.Count, result.Objects.Count);

        for (int i = 0; i < cluster.Objects.Count; ++i)
        {
            AssertDataObjectClusteringEqualDto(
                cluster.Objects[i],
                result.Objects[i],
                coordinates,
                includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that ClusterModel list matches the expected ClusterDto list.
    /// </summary>
    public static void AssertClusterModelListEqualDtoList(
        List<ClusterModel> clusters,
        List<ClusterDto> result,
        List<DataObjectCoordinateModel> coordinates,
        bool includeParameterValues)
    {
        Assert.Equal(clusters.Count, result.Count);

        for (int i = 0; i < clusters.Count; ++i)
        {
            AssertClusterModelEqualDto(
                clusters[i],
                result[i],
                coordinates,
                includeParameterValues);
        }
    }

    /// <summary>
    /// Verifies that DataObjectModel matches the expected DataObjectClusteringAnalysisDto with coordinates.
    /// </summary>
    private static void AssertDataObjectClusteringEqualDto(
        DataObjectModel dataObject,
        DataObjectClusteringAnalysisDto result,
        List<DataObjectCoordinateModel> coordinates,
        bool includeParameterValues)
    {
        Assert.Equal(dataObject.Id, result.Id);
        Assert.Equal(dataObject.Name, result.Name);

        // Assert coordinates
        var coordinate = coordinates.FirstOrDefault(c => c.ObjectId == dataObject.Id);
        var expectedX = coordinate?.X ?? 0.0;
        var expectedY = coordinate?.Y ?? 0.0;

        Assert.Equal(expectedX, result.X);
        Assert.Equal(expectedY, result.Y);

        if (includeParameterValues)
        {
            BaseDomainAnalysisMapperAssertions.AssertParameterValuesEqualDto(
                dataObject.Values,
                result.ParameterValues);
        }
        else
        {
            Assert.Null(result.ParameterValues);
        }
    }
}
