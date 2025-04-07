namespace DataAnalyzeAPI.Services.Analyse.Metrics;

public abstract class BaseDistanceMetric<T>
{
    /// <summary>
    /// Validates input arrays.
    /// Throws exceptions if arrays are null, have different lengths, or are empty.
    /// </summary>
    protected void Validate(T[] valuesA, T[] valuesB)
    {
        if (valuesA == null || valuesB == null)
            throw new ArgumentNullException(valuesA == null ? nameof(valuesA) : nameof(valuesB));

        if (valuesA.Length != valuesB.Length)
            throw new ArgumentException("Values must be the same length.");

        if (valuesA.Length == 0)
            throw new ArgumentException("Values cannot be empty.");
    }
}
