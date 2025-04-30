namespace DataAnalyzeApi.Services.Analyse.Metrics.Categorical;

public class JaccardDistanceMetric : BaseDistanceMetric<int>
{
    /// <summary>
    /// Calculates Jaccard distance between two categorical vectors in one-hot encoding.
    /// Distance is calculated as: 1 - (size of intersection / size of union).
    /// </summary>
    protected override double CalculateDistance(int[] oneHotValuesA, int[] oneHotValuesB)
    {
        Validate(oneHotValuesA, oneHotValuesB);

        int intersection = 0;
        int union = 0;

        for (int i = 0; i < oneHotValuesA.Length; ++i)
        {
            if (oneHotValuesA[i] == 1 && oneHotValuesB[i] == 1)
                ++intersection;

            if (oneHotValuesA[i] == 1 || oneHotValuesB[i] == 1)
                ++union;
        }

        return union == 0 ? 0 : 1.0 - (double)intersection / union;
    }
}
