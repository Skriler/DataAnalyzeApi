using DataAnalyzeAPI.Services.Analyse.Metrics.Numeric;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Metrics.Numeric;

public abstract class NumericDistanceMetricTests
{
    protected abstract INumericDistanceMetric Metric { get; }

    [Theory]
    [InlineData(new double[] { 0.2, 0.6 }, new double[] { 0.2, 0.6 }, 0)] // Identical vectors
    [InlineData(new double[] { 1, 0 }, new double[] { 0, 1 }, 1)] // Orthogonal vectors
    [InlineData(new double[] { 0, 0 }, new double[] { 1, 1 }, 1)] // Zero vector
    public void Calculate_ShouldReturnExpectedDistance(double[] valuesA, double[] valuesB, double expectedDistance)
    {
        // Act
        var distance = Metric.Calculate(valuesA, valuesB);

        // Assert
        Assert.Equal(distance, expectedDistance, precision: 4);
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorIsNull()
    {
        // Arrange
        var valuesA = new double[] { 1, 2, 3 };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Metric.Calculate(null!, valuesA));
        Assert.Throws<ArgumentNullException>(() => Metric.Calculate(valuesA, null!));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var valuesA = new double[] { 1, 2, 3 };
        var valuesB = new double[] { 1, 2 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Metric.Calculate(valuesA, valuesB));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsAreEmpty()
    {
        // Arrange
        var valuesA = new double[0];
        var valuesB = new double[0];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Metric.Calculate(valuesA, valuesB));
    }
}
