using DataAnalyzeApi.Services.Analyse.Metrics;
using DataAnalyzeApi.Services.Analyse.Metrics.Numeric;

namespace DataAnalyzeApi.Unit.Tests.Services.Analyse.Metrics.Numeric;

[Trait("Category", "Unit")]
[Trait("Component", "Metrics")]
[Trait("SubComponent", "Numeric")]
[Trait("Metric", "Euclidean")]
public class EuclideanDistanceMetricTests : NumericDistanceMetricTests
{
    protected override IDistanceMetric<double> Metric => new EuclideanDistanceMetric();

    [Theory]
    [InlineData(new double[] { 1, 0 }, new double[] { 1, 1 }, 0.7071)]
    [InlineData(new double[] { 1, 0 }, new double[] { 0, 0 }, 0.7071)]
    [InlineData(new double[] { 0.2, 0.4 }, new double[] { 0.3, 0.5 }, 0.1)]
    public void Calculate_Specific_ReturnsExpectedDistance(double[] valuesA, double[] valuesB, double expectedDistance)
    {
        var distance = Metric.Calculate(valuesA, valuesB);

        Assert.Equal(expectedDistance, distance, precision: 4);
    }
}
