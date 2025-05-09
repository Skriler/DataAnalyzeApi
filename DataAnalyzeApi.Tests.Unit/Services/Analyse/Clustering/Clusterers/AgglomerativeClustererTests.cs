using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Clusterers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Clusterers;

public class AgglomerativeClustererTests : BaseClustererTests<AgglomerativeClusterer, AgglomerativeSettings>
{
    protected override AgglomerativeClusterer CreateClusterer(
        IDistanceCalculator calculator,
        ClusterNameGenerator generator)
    {
        return new AgglomerativeClusterer(calculator, generator);
    }

    protected override AgglomerativeSettings CreateSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const double threshold = 0.2;

        return new AgglomerativeSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            Threshold: threshold);
    }

    [Theory]
    [MemberData(nameof(AgglomerativeClustererTestData.GetAgglomerativeClusterTestCases), MemberType = typeof(AgglomerativeClustererTestData))]
    public void Calculate_ReturnsExpectedDistance(AgglomerativeClustererTestCase testCase)
    {
        // Arrange
        var dataset = dataFactory.CreateNormalizedDatasetModel(testCase.Objects);

        var settings = new AgglomerativeSettings(
            NumericMetric: default,
            CategoricalMetric: default,
            IncludeParameters: false,
            Threshold: testCase.Threshold);

        SetupDistanceCalculatorMock(testCase.PairwiseDistances!);

        // Act
        var result = clusterer.Cluster(dataset.Objects, settings);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(testCase.ExpectedClusterCount, result.Count);

        var expectedClusterSizes = testCase.ExpectedClusterSizes
            .OrderByDescending(s => s)
            .ToList();

        var actualClusterSizes = result
            .Select(c => c.Objects.Count)
            .OrderByDescending(size => size)
            .ToList();

        Assert.Equal(expectedClusterSizes, actualClusterSizes);
    }
}
