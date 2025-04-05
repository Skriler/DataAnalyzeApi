namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators.NumericMetrics;

public interface INumericDistanceMetric
{
    double Calculate(double[] valuesA, double[] valuesB);
}
