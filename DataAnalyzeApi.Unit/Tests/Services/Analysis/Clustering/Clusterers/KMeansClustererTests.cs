using DataAnalyzeApi.Models.Domain.Clustering.KMeans;
using DataAnalyzeApi.Models.Domain.Dataset;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analysis.Clustering.Helpers;
using DataAnalyzeApi.Services.Analysis.DistanceCalculators;
using DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers.KMeans;
using Moq;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.Clustering.Clusterers;

[Trait("Category", "Unit")]
[Trait("Component", "Analysis")]
[Trait("SubComponent", "Clustering")]
[Trait("Algorithm", "KMeans")]
public class KMeansClustererTests : BaseClustererTests<KMeansClusterer, KMeansSettings>
{
    public KMeansClustererTests()
        : base(CreateKMeansClusterer)
    { }

    private static KMeansClusterer CreateKMeansClusterer(IDistanceCalculator calculator, ClusterNameGenerator generator)
    {
        var centroidCalculatorMock = new Mock<CentroidCalculator>();
        centroidCalculatorMock
            .Setup(c => c.Recalculate(
                It.IsAny<Centroid>(),
                It.IsAny<List<DataObjectModel>>()))
            .Returns((Centroid centroid, List<DataObjectModel> objects) => centroid);

        return new KMeansClusterer(
            calculator,
            generator,
            centroidCalculatorMock.Object);
    }

    private static KMeansSettings CreateKMeansSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric,
        int maxIterations,
        int numberOfClusters)
    {
        return new KMeansSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            MaxIterations: maxIterations,
            NumberOfClusters: numberOfClusters);
    }

    protected override KMeansSettings CreateDefaultSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const int maxIterations = 500;
        const int numberOfClusters = 1;

        return CreateKMeansSettings(
            numericMetric,
            categoricalMetric,
            maxIterations,
            numberOfClusters);
    }

    /// <summary>
    /// Verify that the algorithm throws an exception instead of returning an empty list,
    /// K-Means shouldn't allow more clusters than objects, so in this case an exception is required.
    /// </summary>
    [Fact]
    public override void Cluster_WhenEmptyObjects_ReturnsEmptyList()
    {
        // Arrange
        var emptyObjects = new List<DataObjectModel>();
        var settings = CreateDefaultSettings(default, default);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => clusterer.Cluster(emptyObjects, settings));
    }

    [Theory]
    [MemberData(
        nameof(KMeansClustererTestData.KMeansClustererTestCases),
        MemberType = typeof(KMeansClustererTestData))]
    public void Cluster_ReturnsExpectedDistance(KMeansClustererTestCase testCase)
    {
        var settings = CreateKMeansSettings(
            default,
            default,
            testCase.MaxIterations,
            testCase.NumberOfClusters);

        ClustererReturnsExpectedClusters(testCase, settings);
    }
}
