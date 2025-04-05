namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators.CategoricalMetrics;

public interface ICategoricalDistanceMetric
{
    double Calculate(int[] valuesA, int[] valuesB);
}
