using DataAnalyzeAPI.Services.Analyse.Metrics.Numeric;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Metrics.Numeric;

public class EuclideanDistanceMetricTests : NumericDistanceMetricTests
{
    protected override INumericDistanceMetric Metric => new EuclideanDistanceMetric();

    [Theory]
    [InlineData(new double[] { 1, 0 }, new double[] { 1, 1 }, 0.7071)]
    [InlineData(new double[] { 1, 0 }, new double[] { 0, 0 }, 0.7071)]
    [InlineData(new double[] { 0.2, 0.4 }, new double[] { 0.3, 0.5 }, 0.1)]
    public void Calculate_ShouldReturnExpectedDistance_Specific(double[] valuesA, double[] valuesB, double expectedDistance)
    {
        var distance = Metric.Calculate(valuesA, valuesB);

        Assert.Equal(expectedDistance, distance, precision: 4);
    }
}
