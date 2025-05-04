using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Services.Analyse.Factories.Metric;
using DataAnalyzeApi.Services.Analyse.Metrics;
using Moq;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.DistanceCalculators;

public class DistanceCalculatorTests
{
    private readonly Mock<IMetricFactory> metricFactoryMock;
    private readonly Mock<IDistanceMetric<double>> numericMetricMock;
    private readonly Mock<IDistanceMetric<int>> categoricalMetricMock;
    private readonly DistanceCalculator calculator;

    public DistanceCalculatorTests()
    {
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
        var valuesA = new List<ParameterValueModel>();

        // Act & Assert
        Assert.Throws<VectorNullException>(() => calculator.Calculate(null!, valuesA, default, default));
        Assert.Throws<VectorNullException>(() => calculator.Calculate(valuesA, null!, default, default));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var valuesA = CreateNumericValues(2);
        var valuesB = CreateNumericValues(3);

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
        var result = calculator.Calculate(numericValuesA, numericValuesB, default, default);

        // Assert
        Assert.Equal(expectedDistance, result);
        metricFactoryMock.Verify(f => f.GetNumeric(default), Times.Once);
        metricFactoryMock.Verify(f => f.GetCategorical(It.IsAny<CategoricalDistanceMetricType>()), Times.Never);
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
        var result = calculator.Calculate(categoricalValuesA, categoricalValuesB, default, default);

        // Assert
        Assert.Equal(expectedDistance, result);
        metricFactoryMock.Verify(f => f.GetNumeric(It.IsAny<NumericDistanceMetricType>()), Times.Never);
        metricFactoryMock.Verify(f => f.GetCategorical(default), Times.Once);
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

        // Act
        var result = calculator.Calculate(mixedValuesA, mixedValuesB, default, default);

        // Assert
        Assert.Equal(expectedAverageDistance, result, 3);
        metricFactoryMock.Verify(f => f.GetNumeric(default), Times.Once);
        metricFactoryMock.Verify(f => f.GetCategorical(default), Times.Once);
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
        metricFactoryMock.Verify(f => f.GetNumeric(NumericDistanceMetricType.Manhattan), Times.Once);
        metricFactoryMock.Verify(f => f.GetNumeric(NumericDistanceMetricType.Euclidean), Times.Never);
        metricFactoryMock.Verify(f => f.GetNumeric(NumericDistanceMetricType.Cosine), Times.Never);
        metricFactoryMock.Verify(f => f.GetCategorical(CategoricalDistanceMetricType.Jaccard), Times.Once);
        metricFactoryMock.Verify(f => f.GetCategorical(CategoricalDistanceMetricType.Hamming), Times.Never);
    }

    #region Test Helpers

    /// <summary>
    /// Creates a list of parameter values using a factory function.
    /// </summary>
    private static List<ParameterValueModel> CreateParameterValues(int count, Func<int, ParameterValueModel> factory)
    {
        var result = new List<ParameterValueModel>(count);

        for (int i = 0; i < count; ++i)
        {
            result.Add(factory(i));
        }

        return result;
    }

    /// <summary>
    /// Creates a list of numeric parameter values with normalized values.
    /// </summary>
    private static List<ParameterValueModel> CreateNumericValues(int count) =>
        CreateParameterValues(count, i => new NormalizedNumericValueModel(
            Id: i,
            Value: i.ToString(),
            ParameterId: i,
            Parameter: null!,
            NormalizedValue: (double)i / 10)
        );

    /// <summary>
    /// Creates a list of categorical parameter values with one-hot encoding.
    /// </summary>
    private static List<ParameterValueModel> CreateCategoricalValues(int count, int vectorLength = 3)
    {
        return CreateParameterValues(count, i => {
            var oneHotValues = new int[vectorLength];
            oneHotValues[i % vectorLength] = 1;

            return new NormalizedCategoricalValueModel(
                Id: i,
                Value: i.ToString(),
                ParameterId: i,
                Parameter: null!,
                OneHotValues: oneHotValues);
        });
    }

    #endregion
}
