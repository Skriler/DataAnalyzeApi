using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Clusterers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Clusterers;

public class KMeansClustererTests : BaseClustererTests<KMeansClusterer, KMeansSettings>
{
    protected CentroidCalculator centroidCalculator = new();

    protected override KMeansClusterer CreateClusterer(
        IDistanceCalculator calculator,
        ClusterNameGenerator generator)
    {
        return new KMeansClusterer(calculator, generator, centroidCalculator);
    }

    protected override KMeansSettings CreateSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const int maxIterations = 500;
        const int numberOfClusters = 1;

        return new KMeansSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            MaxIterations: maxIterations,
            NumberOfClusters: numberOfClusters);
    }

    [Fact]
    public override void Cluster_WhenEmptyObjects_ReturnsEmptyList()
    {
        // Arrange
        var emptyObjects = new List<DataObjectModel>();
        var settings = CreateSettings(default, default);

        // Act
        var result = clusterer.Cluster(emptyObjects, settings);

        // Assert
        Assert.Throws<InvalidOperationException>(() => clusterer.Cluster(emptyObjects, settings));
    }

    [Theory]
    [MemberData(nameof(KMeansClustererTestData.GetKMeansClustererTestCases), MemberType = typeof(KMeansClustererTestData))]
    public void Cluster_ReturnsExpectedDistance(KMeansClustererTestCase testCase)
    {
        var settings = new KMeansSettings(
            NumericMetric: default,
            CategoricalMetric: default,
            IncludeParameters: false,
            MaxIterations: testCase.MaxIterations,
            NumberOfClusters: testCase.NumberOfClusters);

        ClustererReturnsExpectedClusters(testCase, settings);
    }
}
