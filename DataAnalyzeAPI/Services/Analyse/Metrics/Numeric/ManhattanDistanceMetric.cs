namespace DataAnalyzeAPI.Services.Analyse.Metrics.Numeric;

public class ManhattanDistanceMetric : BaseDistanceMetric<double>, INumericDistanceMetric
{
    /// <summary>
    /// Calculates normalized Manhattan distance between two numeric vectors.
    /// Distance is calculated as the sum of absolute differences
    /// normalized by the vector length.
    /// </summary>
    public double Calculate(double[] valuesA, double[] valuesB)
    {
        Validate(valuesA, valuesB);

        var distance = 0d;
        for (int i = 0; i < valuesA.Length; i++)
        {
            distance += Math.Abs(valuesA[i] - valuesB[i]);
        }

        return distance / valuesA.Length;
    }
}
