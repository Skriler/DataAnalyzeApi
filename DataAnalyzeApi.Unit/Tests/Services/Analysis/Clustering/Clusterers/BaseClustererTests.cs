using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analysis.Clustering.Helpers;
using DataAnalyzeApi.Services.Analysis.DistanceCalculators;
using DataAnalyzeApi.Unit.Common.Factories.Datasets.Models;
using DataAnalyzeApi.Unit.Common.Models.Analysis;
using DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers;
using Moq;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.Clustering.Clusterers;

/// <summary>
/// Base class for all clustering algorithm tests that provides common setup and test methods.
/// </summary>
/// <typeparam name="TClusterer">The type of clusterer being tested</typeparam>
/// <typeparam name="TSettings">The settings type for the clusterer</typeparam>
[Trait("Category", "Unit")]
[Trait("Component", "Analysis")]
[Trait("SubComponent", "Clustering")]
public abstract class BaseClustererTests<TClusterer, TSettings>
    where TClusterer : BaseClusterer<TSettings>
    where TSettings : BaseClusterSettings
{
    protected readonly DatasetModelFactory datasetModelFactory;
    protected readonly Mock<IDistanceCalculator> distanceCalculatorMock;
    protected readonly Mock<ClusterNameGenerator> nameGeneratorMock;
    protected readonly TClusterer clusterer;

    protected BaseClustererTests(Func<IDistanceCalculator, ClusterNameGenerator, TClusterer> createClusterer)
    {
        datasetModelFactory = new();
        distanceCalculatorMock = new Mock<IDistanceCalculator>();
        nameGeneratorMock = new Mock<ClusterNameGenerator>();

        nameGeneratorMock
            .Setup(g => g.GenerateName(It.IsAny<string>()))
            .Returns<string>(prefix => $"{prefix}-Cluster");

        clusterer = createClusterer(distanceCalculatorMock.Object, nameGeneratorMock.Object);
    }

    [Fact]
    public virtual void Cluster_WhenEmptyObjects_ReturnsEmptyList()
    {
        // Arrange
        var emptyObjects = new List<DataObjectModel>();
        var settings = CreateDefaultSettings(default, default);

        // Act
        var result = clusterer.Cluster(emptyObjects, settings);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public virtual void Cluster_WhenOnlyOneObject_ReturnsSingleCluster()
    {
        // Arrange
        var dataObjects = new List<NormalizedDataObject>
        {
            new()
            {
                NumericValues = [0.2, 0.4],
                CategoricalValues = [[0, 1, 0]],
            },
        };

        var dataset = datasetModelFactory.CreateNormalized(dataObjects);
        var settings = CreateDefaultSettings(default, default);

        // Act
        var result = clusterer.Cluster(dataset.Objects, settings);

        // Assert
        Assert.Single(result);
        Assert.Single(result[0].Objects);
        Assert.Equal(dataset.Objects[0].Name, result[0].Objects[0].Name);
    }

    [Fact]
    public virtual void Cluster_WhenCalculatingDistances_ShouldUseSpecifiedMetrics()
    {
        // Arrange
        var dataObjects = new List<NormalizedDataObject>
        {
            new()
            {
                NumericValues = [0.2, 0.4],
                CategoricalValues = [[0, 1, 0]],
            },

            new()
            {
                NumericValues = [0.6, 0.8],
                CategoricalValues = [[0, 0, 1]],
            },
        };

        var dataset = datasetModelFactory.CreateNormalized(dataObjects);

        var settings = CreateDefaultSettings(
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard);

        // Act
        clusterer.Cluster(dataset.Objects, settings);

        // Assert
        distanceCalculatorMock.Verify(d => d.Calculate(
            It.IsAny<DataObjectModel>(),
            It.IsAny<DataObjectModel>(),
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard),
            Times.AtLeastOnce);

        distanceCalculatorMock.Verify(d => d.Calculate(
            It.IsAny<DataObjectModel>(),
            It.IsAny<DataObjectModel>(),
            It.Is<NumericDistanceMetricType>(m => m != NumericDistanceMetricType.Manhattan),
            It.IsAny<CategoricalDistanceMetricType>()),
            Times.Never);

        distanceCalculatorMock.Verify(d => d.Calculate(
            It.IsAny<DataObjectModel>(),
            It.IsAny<DataObjectModel>(),
            It.IsAny<NumericDistanceMetricType>(),
            It.Is<CategoricalDistanceMetricType>(m => m != CategoricalDistanceMetricType.Jaccard)),
            Times.Never);
    }

    /// <summary>
    /// Executes the clustering test case with the provided settings.
    /// </summary>
    public void ClustererReturnsExpectedClusters(BaseClustererTestCase testCase, TSettings settings)
    {
        // Arrange
        var dataset = datasetModelFactory.CreateNormalized(testCase.Objects);
        SetupDistanceCalculatorMock(testCase.PairwiseDistances);

        // Act
        var result = clusterer.Cluster(dataset.Objects, settings);

        // Assert
        AssertClustersEqualsExpected(testCase, result);
    }

    /// <summary>
    /// Verifies that the clustering result matches the expected outcome
    /// </summary>
    protected virtual void AssertClustersEqualsExpected(BaseClustererTestCase testCase, List<ClusterModel> result)
    {
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

    /// <summary>
    /// Sets up the mock IDistanceCalculator to return predefined distances between object pairs.
    /// </summary>
    protected void SetupDistanceCalculatorMock(List<ObjectPairDistance> pairwiseDistances)
    {
        var distanceLookup = new Dictionary<(long, long), double>();

        foreach (var pair in pairwiseDistances)
        {
            distanceLookup[(pair.ObjectAIndex, pair.ObjectBIndex)] = pair.Distance;
            distanceLookup[(pair.ObjectBIndex, pair.ObjectAIndex)] = pair.Distance;
        }

        distanceCalculatorMock
            .Setup(d => d.Calculate(
                It.IsAny<DataObjectModel>(),
                It.IsAny<DataObjectModel>(),
                It.IsAny<NumericDistanceMetricType>(),
                It.IsAny<CategoricalDistanceMetricType>()))
            .Returns((
                DataObjectModel objA,
                DataObjectModel objB,
                NumericDistanceMetricType _,
                CategoricalDistanceMetricType __) =>
            {
                if (objA.Id == objB.Id)
                {
                    return 0;
                }

                if (distanceLookup.TryGetValue((objA.Id, objB.Id), out double distance))
                {
                    return distance;
                }

                // Throw error if there is no pair distance in the test case.
                throw new InvalidOperationException($"No mock distance defined for objects with IDs {objA.Id} and {objB.Id}.");
            });
    }

    protected abstract TSettings CreateDefaultSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric);
}
