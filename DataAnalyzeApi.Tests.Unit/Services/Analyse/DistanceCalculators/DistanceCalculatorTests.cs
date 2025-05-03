using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Services.Analyse.Metrics;
using Moq;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.DistanceCalculators;

public class DistanceCalculatorTests
{
    private readonly Mock<MetricFactory> metricFactoryMock;
    private readonly Mock<IDistanceMetric<double>> numericMetricMock;
    private readonly Mock<IDistanceMetric<int>> categoricalMetricMock;
    private readonly IDistanceCalculator calculator;

    public DistanceCalculatorTests()
    {
        metricFactoryMock = new Mock<MetricFactory>();
        numericMetricMock = new Mock<IDistanceMetric<double>>();
        categoricalMetricMock = new Mock<IDistanceMetric<int>>();

        metricFactoryMock
            .Setup(f => f.CreateNumericMetric(It.IsAny<NumericDistanceMetricType>()))
            .Returns(numericMetricMock.Object);

        metricFactoryMock
            .Setup(f => f.CreateCategoricalMetric(It.IsAny<CategoricalDistanceMetricType>()))
            .Returns(categoricalMetricMock.Object);

        calculator = new DistanceCalculator(metricFactoryMock.Object);
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorIsNull()
    {
        // Arrange
        var valuesA = new List<ParameterValueModel>();

        // Act & Assert
        Assert.Throws<VectorNullException>(() => calculator.Calculate(null!, valuesA, default, default));
        Assert.Throws<VectorNullException>(() => calculator.Calculate(valuesA, null!, default, default));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var valuesA = CreateParameterValues(2);
        var valuesB = CreateParameterValues(3);

        // Act & Assert
        Assert.Throws<VectorLengthMismatchException>(() => calculator.Calculate(valuesA, valuesB, default, default));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsAreEmpty()
    {
        // Arrange
        var empty = new List<ParameterValueModel>();

        // Act & Assert
        Assert.Throws<EmptyVectorException>(() => calculator.Calculate(empty, empty, default, default));
    }

    [Fact]
    public void Calculate_WhenOnlyNumericParameters_ReturnsNumericDistanceOnly()
    {
        // Arrange
        const double expectedDistance = 0.5;
        var numericValuesA = CreateNumericValues(3);
        var numericValuesB = CreateNumericValues(3);

        numericMetricMock
            .Setup(m => m.Calculate(It.IsAny<double[]>(), It.IsAny<double[]>()))
            .Returns(expectedDistance);

        // Act
        var result = calculator.Calculate(
            numericValuesA,
            numericValuesB,
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard);

        // Assert
        Assert.Equal(expectedDistance, result);
        metricFactoryMock.Verify(f => f.CreateNumericMetric(NumericDistanceMetricType.Manhattan), Times.Once);
        metricFactoryMock.Verify(f => f.CreateCategoricalMetric(It.IsAny<CategoricalDistanceMetricType>()), Times.Never);
    }

    [Fact]
    public void Calculate_WhenOnlyCategoricalParameters_ReturnCategoricalDistanceOnly()
    {
        // Arrange
        const double expectedDistance = 0.3;
        var categoricalValuesA = CreateCategoricalValues(3);
        var categoricalValuesB = CreateCategoricalValues(3);

        categoricalMetricMock
                .Setup(m => m.Calculate(It.IsAny<int[]>(), It.IsAny<int[]>()))
                .Returns(expectedDistance);

        // Act
        var result = calculator.Calculate(
            categoricalValuesA,
            categoricalValuesB,
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard);

        // Assert
        Assert.Equal(expectedDistance, result);
        metricFactoryMock.Verify(f => f.CreateNumericMetric(It.IsAny<NumericDistanceMetricType>()), Times.Never);
        metricFactoryMock.Verify(f => f.CreateCategoricalMetric(CategoricalDistanceMetricType.Jaccard), Times.Once);
    }

    [Fact]
    public void Calculate_WithMixedParameters_ReturnsWeightedAverageDistance()
    {
        // Arrange
        const double numericDistance = 0.8;
        const double categoricalDistance = 0.2;
        const int numericCount = 2;
        const int categoricalCount = 3;

        // Expected weighted average: (0.8*2 + 0.2*3) / 5 = 0.44
        const double expectedAverageDistance = 0.44;

        var mixedValuesA = new List<ParameterValueModel>();
        mixedValuesA.AddRange(CreateNumericValues(numericCount));
        mixedValuesA.AddRange(CreateCategoricalValues(categoricalCount));

        var mixedValuesB = new List<ParameterValueModel>();
        mixedValuesB.AddRange(CreateNumericValues(numericCount));
        mixedValuesB.AddRange(CreateCategoricalValues(categoricalCount));

        numericMetricMock
            .Setup(m => m.Calculate(It.IsAny<double[]>(), It.IsAny<double[]>()))
            .Returns(numericDistance);

        categoricalMetricMock
            .Setup(m => m.Calculate(It.IsAny<int[]>(), It.IsAny<int[]>()))
            .Returns(categoricalDistance);

        numericMetricMock
            .Setup(m => m.Calculate(It.IsAny<double[]>(), It.IsAny<double[]>()))
            .Returns(0.4);

        categoricalMetricMock
            .Setup(m => m.Calculate(It.IsAny<int[]>(), It.IsAny<int[]>()))
            .Returns(0.6);

        // Act
        var result = calculator.Calculate(
            mixedValuesA,
            mixedValuesB,
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard);

        // Assert
        Assert.Equal(expectedAverageDistance, result, 3);
        metricFactoryMock.Verify(f => f.CreateNumericMetric(NumericDistanceMetricType.Manhattan), Times.Once);
        metricFactoryMock.Verify(f => f.CreateCategoricalMetric(CategoricalDistanceMetricType.Jaccard), Times.Once);
    }

    [Fact]
    public void Calculate_WithDifferentMetricTypes_UsesCorrectMetrics()
    {
        // Arrange
        var mixedValuesA = new List<ParameterValueModel>();
        mixedValuesA.AddRange(CreateNumericValues(1));
        mixedValuesA.AddRange(CreateCategoricalValues(1));

        var mixedValuesB = new List<ParameterValueModel>();
        mixedValuesB.AddRange(CreateNumericValues(1));
        mixedValuesB.AddRange(CreateCategoricalValues(1));

        // Act
        calculator.Calculate(
            mixedValuesA,
            mixedValuesB,
            NumericDistanceMetricType.Manhattan,
            CategoricalDistanceMetricType.Jaccard);

        // Assert
        metricFactoryMock.Verify(f => f.CreateNumericMetric(NumericDistanceMetricType.Manhattan), Times.Once);
        metricFactoryMock.Verify(f => f.CreateCategoricalMetric(CategoricalDistanceMetricType.Jaccard), Times.Once);
    }

    private static List<ParameterValueModel> CreateParameterValues(int count)
    {
        var result = new List<ParameterValueModel>();

        for (int i = 0; i < count; ++i)
        {
            var numericValue = new NormalizedNumericValueModel(
                normalizedValue: (double)i / 10,
                parameter: null!,
                value: i.ToString());

            result.Add(numericValue);
        }

        return result;
    }

    private static List<ParameterValueModel> CreateNumericValues(int count) => CreateParameterValues(count);

    private static List<ParameterValueModel> CreateCategoricalValues(int count)
    {
        var result = new List<ParameterValueModel>();

        for (int i = 0; i < count; ++i)
        {
            var categoricalValue = new NormalizedCategoricalValueModel(
                oneHotValues: new int[] { 0, 1, 0 },
                parameter: null!,
                value: i.ToString());

            result.Add(categoricalValue);
        }

        return result;
    }
}
