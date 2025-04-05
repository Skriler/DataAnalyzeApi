namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators.NumericMetrics;

public class ManhattanDistanceMetric : INumericDistanceMetric
{
    public double Calculate(double[] valuesA, double[] valuesB)
    {
        if (valuesA.Length != valuesB.Length)
            throw new ArgumentException("Values must be the same length");

        var distance = 0d;
        for (int i = 0; i < valuesA.Length; i++)
        {
            distance += Math.Abs(valuesA[i] - valuesB[i]);
        }

        return distance / valuesA.Length;
    }
}
