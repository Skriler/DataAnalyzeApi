namespace DataAnalyzeAPI.Services.Analyse.Metrics.Categorical;

public interface ICategoricalDistanceMetric
{
    /// <summary>
    /// Returns a value between 0 and 1.
    /// 0 indicates identical, 1 indicates completely different vectors.
    /// </summary>
    double Calculate(int[] oneHotValuesA, int[] oneHotValuesB);
}
