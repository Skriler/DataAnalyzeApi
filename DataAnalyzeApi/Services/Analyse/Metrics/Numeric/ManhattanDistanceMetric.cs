namespace DataAnalyzeApi.Services.Analyse.Metrics.Numeric;

public class ManhattanDistanceMetric : BaseDistanceMetric<double>
{
    /// <summary>
    /// Calculates normalized Manhattan distance between two numeric vectors.
    /// Distance is calculated as the sum of absolute differences
    /// normalized by the vector length.
    /// </summary>
    protected override double CalculateDistance(double[] valuesA, double[] valuesB)
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
