using DataAnalyzeApi.Services.Analyse.Metrics;
using DataAnalyzeApi.Services.Analyse.Metrics.Categorical;

namespace DataAnalyzeApi.Unit.Tests.Services.Analyse.Metrics.Categorical;

[Trait("Category", "Unit")]
[Trait("Component", "Metrics")]
[Trait("SubComponent", "Categorical")]
[Trait("Metric", "Hamming")]
public class HammingDistanceMetricTests : CategoricalDistanceMetricTests
{
    protected override IDistanceMetric<int> Metric => new HammingDistanceMetric();

    [Theory]
    [InlineData(new int[] { 1, 0, 0 }, new int[] { 0, 1, 0 }, 0.6667)]
    [InlineData(new int[] { 0, 0, 0 }, new int[] { 1, 0, 0 }, 0.3333)]
    [InlineData(new int[] { 1, 1, 0 }, new int[] { 1, 0, 1 }, 0.6667)]
    [InlineData(new int[] { 1, 0, 1, 0 }, new int[] { 1, 1, 0, 0 }, 0.5)]
    public void Calculate_Specific_ReturnsExpectedDistance(int[] valuesA, int[] valuesB, double expectedDistance)
    {
        var distance = Metric.Calculate(valuesA, valuesB);
        Assert.Equal(expectedDistance, distance, precision: 4);
    }
}
