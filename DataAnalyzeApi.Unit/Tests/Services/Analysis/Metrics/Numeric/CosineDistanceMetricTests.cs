using DataAnalyzeApi.Services.Analysis.Metrics;
using DataAnalyzeApi.Services.Analysis.Metrics.Numeric;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.Metrics.Numeric;

[Trait("Category", "Unit")]
[Trait("Component", "Metrics")]
[Trait("SubComponent", "Numeric")]
[Trait("Metric", "Cosine")]
public class CosineDistanceMetricTests : NumericDistanceMetricTests
{
    protected override IDistanceMetric<double> Metric => new CosineDistanceMetric();

    [Theory]
    [InlineData(new double[] { 1, 0 }, new double[] { 1, 1 }, 0.2929)]
    [InlineData(new double[] { 1, 0 }, new double[] { 0, 0 }, 1)]
    [InlineData(new double[] { 0.2, 0.4 }, new double[] { 0.3, 0.5 }, 0.0029)]
    public void Calculate_Specific_ReturnsExpectedDistance(double[] valuesA, double[] valuesB, double expectedDistance)
    {
        var distance = Metric.Calculate(valuesA, valuesB);

        Assert.Equal(expectedDistance, distance, precision: 4);
    }
}
