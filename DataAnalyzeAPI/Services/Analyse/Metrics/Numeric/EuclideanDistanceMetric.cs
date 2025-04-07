namespace DataAnalyzeAPI.Services.Analyse.Metrics.Numeric;

public class EuclideanDistanceMetric : BaseDistanceMetric<double>, INumericDistanceMetric
{
    /// <summary>
    /// Calculates normalized Euclidean distance between two numeric vectors.
    /// Distance is calculated as the square root of the sum of squared differences
    /// normalized by the maximum possible distance.
    /// </summary>
    public double Calculate(double[] valuesA, double[] valuesB)
    {
        Validate(valuesA, valuesB);

        var distanceSquared = 0d;
        for (int i = 0; i < valuesA.Length; ++i)
        {
            distanceSquared += Math.Pow(valuesA[i] - valuesB[i], 2);
        }

        var distance = Math.Sqrt(distanceSquared);
        var maxDistance = Math.Sqrt(valuesA.Length);
        return distance / maxDistance;
    }
}
