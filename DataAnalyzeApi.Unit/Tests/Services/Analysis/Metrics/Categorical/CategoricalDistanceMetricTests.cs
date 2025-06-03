using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Services.Analysis.Metrics;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.Metrics.Categorical;

[Trait("Category", "Unit")]
[Trait("Component", "Metrics")]
[Trait("SubComponent", "Categorical")]
public abstract class CategoricalDistanceMetricTests
{
    protected abstract IDistanceMetric<int> Metric { get; }

    [Theory]
    [InlineData(new int[] { 1, 0, 0 }, new int[] { 1, 0, 0 }, 0)] // Identical vectors
    [InlineData(new int[] { 1, 1, 1 }, new int[] { 0, 0, 0 }, 1.0)] // All different
    public void Calculate_ReturnsExpectedDistance(int[] valuesA, int[] valuesB, double expectedDistance)
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
        Assert.Throws<VectorNullException>(() => Metric.Calculate(null!, valuesA));
        Assert.Throws<VectorNullException>(() => Metric.Calculate(valuesA, null!));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsHaveDifferentLengths()
    {
        // Arrange
        var valuesA = new int[] { 1, 0, 0 };
        var valuesB = new int[] { 1, 0 };

        // Act & Assert
        Assert.Throws<VectorLengthMismatchException>(() => Metric.Calculate(valuesA, valuesB));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenVectorsAreEmpty()
    {
        // Arrange
        var valuesA = new int[0];
        var valuesB = new int[0];

        // Act & Assert
        Assert.Throws<EmptyVectorException>(() => Metric.Calculate(valuesA, valuesB));
    }
}
