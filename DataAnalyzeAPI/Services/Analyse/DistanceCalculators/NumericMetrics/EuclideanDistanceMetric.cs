namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators.NumericMetrics;

public class EuclideanDistanceMetric : INumericDistanceMetric
{
    public double Calculate(double[] valuesA, double[] valuesB)
    {
        if (valuesA.Length != valuesB.Length)
            throw new ArgumentException("Values must be the same length");

        var distanceSquared = 0d;
        for (int i = 0; i < valuesA.Length; ++i)
        {
            distanceSquared += Math.Pow(valuesA[i] - valuesB[i], 2);
        }

        var distance = Math.Sqrt(distanceSquared);
        return distance / Math.Sqrt(valuesA.Length);
    }
}
