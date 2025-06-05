namespace DataAnalyzeApi.Services.Analysis.Metrics;

public interface IDistanceMetric<T>
{
    double Calculate(T[] a, T[] b);
}
