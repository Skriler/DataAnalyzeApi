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

        int differences = 0;

        for (int i = 0; i < oneHotValuesA.Length; ++i)
        {
            if (oneHotValuesA[i] == oneHotValuesB[i])
                continue;

            ++differences;
        }

        return (double)differences / oneHotValuesA.Length;
    }
}
