using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;
using Moq;
using System;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Clusterers;

public abstract class BaseClustererTests<TClusterer, TSettings>
    where TClusterer : BaseClusterer<TSettings>
    where TSettings : BaseClusterSettings
{
    protected readonly TestDataFactory dataFactory;
    protected readonly Mock<IDistanceCalculator> distanceCalculatorMock;
    protected readonly Mock<ClusterNameGenerator> nameGeneratorMock;
    protected readonly TClusterer clusterer;

    protected BaseClustererTests()
    {
        dataFactory = new();
        distanceCalculatorMock = new Mock<IDistanceCalculator>();
        nameGeneratorMock = new Mock<ClusterNameGenerator>();

        nameGeneratorMock
            .Setup(g => g.GenerateName(It.IsAny<string>()))
            .Returns<string>(prefix => $"{prefix}-Cluster");

        clusterer = CreateClusterer(distanceCalculatorMock.Object, nameGeneratorMock.Object);
    }

    [Fact]
    public void Cluster_WhenEmptyObjects_ReturnsEmptyList()
    {
        // Arrange
        var emptyObjects = new List<DataObjectModel>();
        var settings = CreateSettings(default, default);

        // Act
        var result = clusterer.Cluster(emptyObjects, settings);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Cluster_WhenOnlyOneObject_ReturnsSingleCluster()
    {
        // Arrange
        var dataObjects = new List<NormalizedDataObject>()
        {
            new NormalizedDataObject()
            {
                NumericValues = { 0.2, 0.4 },
                CategoricalValues = { new[] { 0, 1, 0 } }
            }
        };

        var dataset = dataFactory.CreateNormalizedDatasetModel(dataObjects);
        var settings = CreateSettings(default, default);

        // Act
        var result = clusterer.Cluster(dataset.Objects, settings);

        // Assert
        Assert.Single(result);
        Assert.Single(result[0].Objects);
        Assert.Equal(dataset.Objects[0], result[0].Objects[0]);
    }

    [Fact]
    public void Cluster_WhenCalculatingDistances_ShouldUseSpecifiedMetrics()
    {
        // Arrange
        var dataObjects = new List<NormalizedDataObject>()
        {
            new NormalizedDataObject
            {
                NumericValues = { 0.2, 0.4 },
                CategoricalValues = { new[] { 0, 1, 0 } }
            },

            new NormalizedDataObject
            {
                NumericValues = { 0.6, 0.8 },
                CategoricalValues = { new[] { 0, 0, 1 } }
            }
        };

        var dataset = dataFactory.CreateNormalizedDatasetModel(dataObjects);

        var settings = CreateSettings(
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

    protected abstract TClusterer CreateClusterer(
        IDistanceCalculator calculator,
        ClusterNameGenerator generator);

    protected abstract TSettings CreateSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric);

    /// <summary>
    /// Sets up the mock distance calculator to return predefined distances between object pairs.
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
                NumericDistanceMetricType numericType,
                CategoricalDistanceMetricType categoricalType) =>
            {
                if (distanceLookup.TryGetValue((objA.Id, objB.Id), out double distance))
                {
                    return distance;
                }

                //TODO: remove
                throw new InvalidOperationException($"No mock distance defined for objects with IDs {objA.Id} and {objB.Id}.");
            });
    }
}
