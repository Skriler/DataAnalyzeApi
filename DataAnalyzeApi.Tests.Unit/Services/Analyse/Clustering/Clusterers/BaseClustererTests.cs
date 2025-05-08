using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using Moq;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Clusterers;

public abstract class BaseClustererTests<TClusterer, TSettings>
    where TClusterer : BaseClusterer<TSettings>
    where TSettings : BaseClusterSettings
{
    protected readonly Mock<IDistanceCalculator> distanceCalculatorMock;
    protected readonly Mock<ClusterNameGenerator> nameGeneratorMock;
    protected readonly TClusterer clusterer;

    protected BaseClustererTests()
    {
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
        var objects = new List<DataObjectModel> { CreateDataObjectModel(1) };
        var settings = CreateSettings(default, default);

        // Act
        var result = clusterer.Cluster(objects, settings);

        // Assert
        Assert.Single(result);
        Assert.Single(result[0].Objects);
        Assert.Equal(objects[0], result[0].Objects[0]);
    }

    [Fact]
    public void Cluster_WhenCalculatingDistances_ShouldUseSpecifiedMetrics()
    {
        // Arrange
        var objects = new List<DataObjectModel>
        {
            CreateDataObjectModel(1),
            CreateDataObjectModel(2)
        };

        var settings = CreateSettings(
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard);

        // Act
        clusterer.Cluster(objects, settings);

        // Assert
        distanceCalculatorMock.Verify(d => d.Calculate(
            It.IsAny<List<ParameterValueModel>>(),
            It.IsAny<List<ParameterValueModel>>(),
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard),
            Times.AtLeastOnce);

        distanceCalculatorMock.Verify(d => d.Calculate(
            It.IsAny<List<ParameterValueModel>>(),
            It.IsAny<List<ParameterValueModel>>(),
            It.Is<NumericDistanceMetricType>(m => m != NumericDistanceMetricType.Manhattan),
            It.IsAny<CategoricalDistanceMetricType>()),
            Times.Never);

        distanceCalculatorMock.Verify(d => d.Calculate(
            It.IsAny<List<ParameterValueModel>>(),
            It.IsAny<List<ParameterValueModel>>(),
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

    #region Test Helpers

    /// <summary>
    /// Creates a DataObjectModel with a specified ID and mock values.
    /// </summary>
    protected static DataObjectModel CreateDataObjectModel(int id)
    {
        var values = new List<ParameterValueModel>
        {
            new NormalizedNumericValueModel(
                Id: id,
                Value: id.ToString(),
                ParameterId: id,
                Parameter: null!,
                NormalizedValue: 0.5),

            new NormalizedCategoricalValueModel(
                Id: id + 100,
                Value: id.ToString(),
                ParameterId: id + 100,
                Parameter: null!,
                OneHotValues: new[] { 0, 1, 0 })
        };

        return new DataObjectModel(
            Id: id,
            Name: id.ToString(),
            Values: values);
    }

    #endregion
}
