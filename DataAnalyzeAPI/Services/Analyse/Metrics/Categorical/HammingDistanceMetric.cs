namespace DataAnalyzeAPI.Services.Analyse.Metrics.Categorical;

public class HammingDistanceMetric : BaseDistanceMetric<int>, ICategoricalDistanceMetric
{
    /// <summary>
    /// Calculates normalized Hamming distance between two categorical vectors.
    /// Distance is calculated as the proportion of differing elements between vectors.
    /// </summary>
    public double Calculate(int[] oneHotValuesA, int[] oneHotValuesB)
    {
        Validate(oneHotValuesA, oneHotValuesB);

        var differences = oneHotValuesA.Except(oneHotValuesB).Count()
            + oneHotValuesB.Except(oneHotValuesA).Count();

        return differences / (double)oneHotValuesA.Length;
    }
}
