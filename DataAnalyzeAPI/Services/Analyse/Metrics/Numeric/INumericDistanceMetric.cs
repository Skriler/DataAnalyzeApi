namespace DataAnalyzeAPI.Services.Analyse.Metrics.Numeric;

public interface INumericDistanceMetric
{
    /// <summary>
    /// Returns a value between 0 and 1.
    /// 0 indicates identical, 1 indicates completely different vectors.
    /// </summary>
    double Calculate(double[] valuesA, double[] valuesB);
}
