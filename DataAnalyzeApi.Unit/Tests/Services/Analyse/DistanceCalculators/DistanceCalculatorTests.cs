using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Services.Analyse.Factories.Metric;
using DataAnalyzeApi.Services.Analyse.Metrics;
using DataAnalyzeApi.Unit.Common.Factories.Models;
using DataAnalyzeApi.Unit.Common.Models.Analyse;
using Moq;

namespace DataAnalyzeApi.Unit.Tests.Services.Analyse.DistanceCalculators;

[Trait("Category", "Unit")]
[Trait("Component", "Analyse")]
[Trait("SubComponent", "DistanceCalculator")]
public class DistanceCalculatorTests
{
    private readonly DataObjectModelFactory dataObjectModelFactory;
    private readonly Mock<IMetricFactory> metricFactoryMock;
    private readonly Mock<IDistanceMetric<double>> numericMetricMock;
    private readonly Mock<IDistanceMetric<int>> categoricalMetricMock;
    private readonly DistanceCalculator calculator;

    public DistanceCalculatorTests()
    {
        dataObjectModelFactory = new();
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
        var objectB = new DataObjectModel(0, string.Empty, new());

        // Act & Assert
        Assert.Throws<VectorNullException>(() => calculator.Calculate(objectA, objectB, default, default));
        Assert.Throws<VectorNullException>(() => calculator.Calculate(objectB, objectA, default, default));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var objectNumericsA = new NormalizedDataObject
        {
            NumericValues = [0.2, 0.4],
        };
        var objectNumericsB = new NormalizedDataObject
        {
            NumericValues = [0.6, 0.8, 0.3],
        };

        var objectA = dataObjectModelFactory.CreateNormalized(objectNumericsA);
        var objectB = dataObjectModelFactory.CreateNormalized(objectNumericsB);

        // Act & Assert
        Assert.Throws<VectorLengthMismatchException>(() => calculator.Calculate(objectA, objectB, default, default));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsAreEmpty()
    {
        // Arrange
        var objectEmpty = new NormalizedDataObject();
        var objectA = dataObjectModelFactory.CreateNormalized(objectEmpty);

        // Act & Assert
        Assert.Throws<EmptyVectorException>(() => calculator.Calculate(objectA, objectA, default, default));
    }

    [Fact]
    public void Calculate_WhenOnlyNumericParameters_ReturnsNumericDistanceOnly()
    {
        // Arrange
        var objectNumericsA = new NormalizedDataObject
        {
            NumericValues = [0.1, 0.3, 0.8],
        };
        var objectNumericsB = new NormalizedDataObject
        {
            NumericValues = [0.6, 0.8, 0.3],
        };
        const double expectedDistance = 0.5;

        var objectA = dataObjectModelFactory.CreateNormalized(objectNumericsA);
        var objectB = dataObjectModelFactory.CreateNormalized(objectNumericsB);

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
        var objectCategoricalA = new NormalizedDataObject
        {
            CategoricalValues = [[1, 0], [1, 0]],
        };
        var objectCategoricalB = new NormalizedDataObject
        {
            CategoricalValues = [[0, 1], [1, 1]],
        };
        const double expectedDistance = 0.3;

        var objectA = dataObjectModelFactory.CreateNormalized(objectCategoricalA);
        var objectB = dataObjectModelFactory.CreateNormalized(objectCategoricalB);

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
        var objectMixedA = new NormalizedDataObject
        {
            NumericValues = [0.4, 0.7],
            CategoricalValues = [[0, 1], [1, 1, 0], [1, 1]],
        };
        var objectMixedB = new NormalizedDataObject
        {
            NumericValues = [0.1, 1.0],
            CategoricalValues = [[1, 0],[0, 1, 1], [0, 1]],
        };

        const double numericDistance = 0.8;
        const double categoricalDistance = 0.2;
        // Expected weighted average: (0.8*2 + 0.2*3) / 5 = 0.44
        const double expectedAverageDistance = 0.44;

        var objectA = dataObjectModelFactory.CreateNormalized(objectMixedA);
        var objectB = dataObjectModelFactory.CreateNormalized(objectMixedB);

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
        var objectMixedA = new NormalizedDataObject
        {
            NumericValues = [0.6, 0.4],
            CategoricalValues = [[1, 1, 1]],
        };
        var objectMixedB = new NormalizedDataObject
        {
            NumericValues = [0.2, 0.3],
            CategoricalValues = [[0, 1, 1]],
        };

        var objectA = dataObjectModelFactory.CreateNormalized(objectMixedA);
        var objectB = dataObjectModelFactory.CreateNormalized(objectMixedB);

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
