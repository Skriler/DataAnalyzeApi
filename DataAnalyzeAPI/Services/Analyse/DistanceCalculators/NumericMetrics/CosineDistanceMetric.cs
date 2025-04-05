namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators.NumericMetrics;

public class CosineDistanceMetric : INumericDistanceMetric
{
    public double Calculate(double[] valuesA, double[] valuesB)
    {
        if (valuesA.Length != valuesB.Length)
            throw new ArgumentException("Values must be the same length");

        var dotProduct = valuesA
            .Zip(valuesB, (x, y) => x * y)
            .Sum();

        var firstMagnitude = valuesA
            .Sum(x => x * x);

        var secondMagnitude = valuesB
            .Sum(y => y * y);

        if (firstMagnitude == 0 || secondMagnitude == 0)
            return 1;

        var cosineSimilarity = dotProduct / (Math.Sqrt(firstMagnitude) * Math.Sqrt(secondMagnitude));

        return 1 - cosineSimilarity;
    }
}
