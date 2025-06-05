namespace DataAnalyzeApi.Services.Analysis.Metrics.Categorical;

public class HammingDistanceMetric : BaseDistanceMetric<int>
{
    /// <summary>
    /// Calculates normalized Hamming distance between two categorical vectors.
    /// Distance is calculated as the proportion of differing elements between vectors.
    /// </summary>
    protected override double CalculateDistance(int[] oneHotValuesA, int[] oneHotValuesB)
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
