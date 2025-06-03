using DataAnalyzeApi.Exceptions.Vector;

namespace DataAnalyzeApi.Services.Analysis.Metrics;

public abstract class BaseDistanceMetric<T> : IDistanceMetric<T>
{
    /// <summary>
    /// Returns a value between 0 and 1.
    /// 0 indicates identical, 1 indicates completely different vectors.
    /// </summary>
    public double Calculate(T[] a, T[] b)
    {
        Validate(a, b);

        return CalculateDistance(a, b);
    }

    /// <summary>
    /// Validates input arrays.
    /// Throws exceptions if arrays are null, have different lengths, or are empty.
    /// </summary>
    protected void Validate(T[] valuesA, T[] valuesB)
    {
        if (valuesA == null || valuesB == null)
            throw new VectorNullException();

        if (valuesA.Length != valuesB.Length)
            throw new VectorLengthMismatchException();

        if (valuesA.Length == 0)
            throw new EmptyVectorException();
    }

    /// <summary>
    /// Performs the actual distance calculations.
    /// </summary>
    protected abstract double CalculateDistance(T[] a, T[] b);
}
