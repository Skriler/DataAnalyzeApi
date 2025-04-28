using DataAnalyzeAPI.Services.Analyse.Metrics.Categorical;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Metrics.Categorical;

public abstract class CategoricalDistanceMetricTests
{
    protected abstract ICategoricalDistanceMetric Metric { get; }

    [Theory]
    [InlineData(new int[] { 1, 0, 0 }, new int[] { 1, 0, 0 }, 0)] // Identical vectors
    [InlineData(new int[] { 1, 1, 1 }, new int[] { 0, 0, 0 }, 1.0)] // All different
    public void Calculate_ShouldReturnExpectedDistance(int[] valuesA, int[] valuesB, double expectedDistance)
    {
        // Act
        var distance = Metric.Calculate(valuesA, valuesB);

        // Assert
        Assert.Equal(expectedDistance, distance, precision: 4);
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorIsNull()
    {
        // Arrange
        var valuesA = new int[] { 1, 0, 0 };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Metric.Calculate(null!, valuesA));
        Assert.Throws<ArgumentNullException>(() => Metric.Calculate(valuesA, null!));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var valuesA = new int[] { 1, 0, 0 };
        var valuesB = new int[] { 1, 0 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Metric.Calculate(valuesA, valuesB));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsAreEmpty()
    {
        // Arrange
        var valuesA = new int[0];
        var valuesB = new int[0];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Metric.Calculate(valuesA, valuesB));
    }
}
