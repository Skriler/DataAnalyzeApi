namespace DataAnalyzeAPI.Services.Analyse.Metrics.Categorical;

public class JaccardDistanceMetric : BaseDistanceMetric<int>, ICategoricalDistanceMetric
{
    /// <summary>
    /// Calculates Jaccard distance between two categorical vectors in one-hot encoding.
    /// Distance is calculated as: 1 - (size of intersection / size of union).
    /// </summary>
    public double Calculate(int[] oneHotValuesA, int[] oneHotValuesB)
    {
        Validate(oneHotValuesA, oneHotValuesB);

        var intersection = oneHotValuesA.Intersect(oneHotValuesB).Count();
        var union = oneHotValuesA.Union(oneHotValuesB).Count();

        return 1.0 - (double)intersection / union;
    }
}
