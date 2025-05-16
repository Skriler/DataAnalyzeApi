using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Services.Analyse.Factories.Metric;
using DataAnalyzeApi.Services.Analyse.Metrics;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers.Factories.Models;
using Moq;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.DistanceCalculators;

public class DistanceCalculatorTests
{
    private readonly DatasetModelFactory datasetModelFactory;
    private readonly Mock<IMetricFactory> metricFactoryMock;
    private readonly Mock<IDistanceMetric<double>> numericMetricMock;
    private readonly Mock<IDistanceMetric<int>> categoricalMetricMock;
    private readonly DistanceCalculator calculator;

    public DistanceCalculatorTests()
    {
        datasetModelFactory = new();
        metricFactoryMock = new Mock<IMetricFactory>();
        numericMetricMock = new Mock<IDistanceMetric<double>>();
        categoricalMetricMock = new Mock<IDistanceMetric<int>>();

        metricFactoryMock
            .Setup(f => f.GetNumeric(It.IsAny<NumericDistanceMetricType>()))
            .Returns(numericMetricMock.Object);

        metricFactoryMock
            .Setup(f => f.GetCategorical(It.IsAny<CategoricalDistanceMetricType>()))
            .Returns(categoricalMetricMock.Object);

        calculator = new DistanceCalculator(metricFactoryMock.Object);
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorIsNull()
    {
        // Arrange
        var objectA = new DataObjectModel(0, string.Empty, null!);
        var objectB = new DataObjectModel(0, string.Empty, new List<ParameterValueModel>());

        // Act & Assert
        Assert.Throws<VectorNullException>(() => calculator.Calculate(objectA, objectB, default, default));
        Assert.Throws<VectorNullException>(() => calculator.Calculate(objectB, objectA, default, default));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var objectNumericsA = new NormalizedDataObject() { NumericValues = { 0.2, 0.4 } };
        var objectNumericsB = new NormalizedDataObject() { NumericValues = { 0.6, 0.8, 0.3 } };

        var objectA = datasetModelFactory.CreateNormalizedDataObjectModel(objectNumericsA);
        var objectB = datasetModelFactory.CreateNormalizedDataObjectModel(objectNumericsB);

        // Act & Assert
        Assert.Throws<VectorLengthMismatchException>(() => calculator.Calculate(objectA, objectB, default, default));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsAreEmpty()
    {
        // Arrange
        var objectEmpty = new NormalizedDataObject();
        var objectA = datasetModelFactory.CreateNormalizedDataObjectModel(objectEmpty);

        // Act & Assert
        Assert.Throws<EmptyVectorException>(() => calculator.Calculate(objectA, objectA, default, default));
    }

    [Fact]
    public void Calculate_WhenOnlyNumericParameters_ReturnsNumericDistanceOnly()
    {
        // Arrange
        var objectNumericsA = new NormalizedDataObject() { NumericValues = { 0.1, 0.3, 0.8 } };
        var objectNumericsB = new NormalizedDataObject() { NumericValues = { 0.6, 0.8, 0.3 } };
        const double expectedDistance = 0.5;

        var objectA = datasetModelFactory.CreateNormalizedDataObjectModel(objectNumericsA);
        var objectB = datasetModelFactory.CreateNormalizedDataObjectModel(objectNumericsB);

        numericMetricMock
            .Setup(m => m.Calculate(It.IsAny<double[]>(), It.IsAny<double[]>()))
            .Returns(expectedDistance);

        // Act
        var result = calculator.Calculate(objectA, objectB, default, default);

        // Assert
        Assert.Equal(expectedDistance, result, precision: 4);
        metricFactoryMock.Verify(f => f.GetNumeric(default), Times.Once);
        metricFactoryMock.Verify(f => f.GetCategorical(It.IsAny<CategoricalDistanceMetricType>()), Times.Never);
    }

    [Fact]
    public void Calculate_WhenOnlyCategoricalParameters_ReturnsCategoricalDistanceOnly()
    {
        // Arrange
        var objectCategoricalA = new NormalizedDataObject()
        {
            CategoricalValues = { new[] { 1, 0 }, new[] { 1, 0 } }
        };
        var objectCategoricalB = new NormalizedDataObject()
        {
            CategoricalValues = { new[] { 0, 1 }, new[] { 1, 1 } }
        };
        const double expectedDistance = 0.3;

        var objectA = datasetModelFactory.CreateNormalizedDataObjectModel(objectCategoricalA);
        var objectB = datasetModelFactory.CreateNormalizedDataObjectModel(objectCategoricalB);

        categoricalMetricMock
                .Setup(m => m.Calculate(It.IsAny<int[]>(), It.IsAny<int[]>()))
                .Returns(expectedDistance);

        // Act
        var result = calculator.Calculate(objectA, objectB, default, default);

        // Assert
        Assert.Equal(expectedDistance, result, precision: 4);
        metricFactoryMock.Verify(f => f.GetNumeric(It.IsAny<NumericDistanceMetricType>()), Times.Never);
        metricFactoryMock.Verify(f => f.GetCategorical(default), Times.Once);
    }

    [Fact]
    public void Calculate_WithMixedParameters_ReturnsWeightedAverageDistance()
    {
        // Arrange
        var objectMixedA = new NormalizedDataObject()
        {
            NumericValues = { 0.4, 0.7 },
            CategoricalValues = { new[] { 0, 1 }, new[] { 1, 1, 0 }, new[] { 1, 1 } }
        };
        var objectMixedB = new NormalizedDataObject()
        {
            NumericValues = { 0.1, 1.0 },
            CategoricalValues = { new[] { 1, 0 }, new[] { 0, 1, 1 }, new[] { 0, 1 } }
        };

        const double numericDistance = 0.8;
        const double categoricalDistance = 0.2;
        // Expected weighted average: (0.8*2 + 0.2*3) / 5 = 0.44
        const double expectedAverageDistance = 0.44;

        var objectA = datasetModelFactory.CreateNormalizedDataObjectModel(objectMixedA);
        var objectB = datasetModelFactory.CreateNormalizedDataObjectModel(objectMixedB);

        numericMetricMock
            .Setup(m => m.Calculate(It.IsAny<double[]>(), It.IsAny<double[]>()))
            .Returns(numericDistance);

        categoricalMetricMock
            .Setup(m => m.Calculate(It.IsAny<int[]>(), It.IsAny<int[]>()))
            .Returns(categoricalDistance);

        // Act
        var result = calculator.Calculate(objectA, objectB, default, default);

        // Assert
        Assert.Equal(expectedAverageDistance, result, precision: 4);
        metricFactoryMock.Verify(f => f.GetNumeric(default), Times.Once);
        metricFactoryMock.Verify(f => f.GetCategorical(default), Times.Once);
    }

    [Fact]
    public void Calculate_WithDifferentMetricTypes_UsesCorrectMetrics()
    {
        // Arrange
        var objectMixedA = new NormalizedDataObject()
        {
            NumericValues = { 0.6, 0.4 },
            CategoricalValues = { new[] { 1, 1, 1 } }
        };
        var objectMixedB = new NormalizedDataObject()
        {
            NumericValues = { 0.2, 0.3 },
            CategoricalValues = { new[] { 0, 1, 1 } }
        };

        var objectA = datasetModelFactory.CreateNormalizedDataObjectModel(objectMixedA);
        var objectB = datasetModelFactory.CreateNormalizedDataObjectModel(objectMixedB);

        // Act
        calculator.Calculate(
            objectA,
            objectB,
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard);

        // Assert
        metricFactoryMock.Verify(f => f.GetNumeric(NumericDistanceMetricType.Manhattan), Times.Once);
        metricFactoryMock.Verify(f => f.GetNumeric(NumericDistanceMetricType.Euclidean), Times.Never);
        metricFactoryMock.Verify(f => f.GetNumeric(NumericDistanceMetricType.Cosine), Times.Never);
        metricFactoryMock.Verify(f => f.GetCategorical(CategoricalDistanceMetricType.Jaccard), Times.Once);
        metricFactoryMock.Verify(f => f.GetCategorical(CategoricalDistanceMetricType.Hamming), Times.Never);
    }
}
