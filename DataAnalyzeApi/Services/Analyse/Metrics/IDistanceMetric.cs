namespace DataAnalyzeApi.Services.Analyse.Metrics;

public interface IDistanceMetric<T>
{
    double Calculate(T[] a, T[] b);
}
