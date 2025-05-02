using DataAnalyzeApi.Models.Config.Clustering;
using DataAnalyzeApi.Models.Domain.Clustering.KMeans;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeApi.Services.Analyse.Clustering.Helpers;

public class CentroidCalculator
{
    /// <summary>
    /// Threshold used to convert averaged one-hot values to 1 or 0.
    /// If the average is greater than or equal to the threshold, set to 1; otherwise, 0.
    /// </summary>
    private const double OneHotThreshold = KMeansConfig.CentroidConfig.OneHotThreshold;

    /// <summary>
    /// Stores the cumulative sum of numeric parameter values by index.
    /// </summary>
    private readonly Dictionary<int, double> numericSums = new();

    /// <summary>
    /// Stores the cumulative sum of categorical one-hot encoded values by index.
    /// </summary>
    private readonly Dictionary<int, int[]> categoricalSums = new();

    /// <summary>
    /// Recalculates the centroid by averaging its parameters with the provided merge nodes.
    /// </summary>
    public Centroid Recalculate(Centroid centroid, List<DataObjectModel> mergeObjects)
    {
        if (mergeObjects.Count == 0)
            return centroid;

        Validate(centroid, mergeObjects);

        ExtractValues(centroid);
        CalculateValuesSums(mergeObjects);

        var newValues = ApplyAverages(centroid, mergeObjects.Count + 1);
        return new Centroid(newValues);
    }

    /// <summary>
    /// Validates that all nodes have the same number of parameters as the centroid.
    /// </summary>
    private void Validate(Centroid centroid, List<DataObjectModel> mergeObjects)
    {
        if (mergeObjects.Any(obj => obj.Values.Count != centroid.Values.Count))
            throw new ArgumentException("Object values count doesn't match centroid's");
    }

    /// <summary>
    /// Extracts and stores the initial values from the centroid for further averaging.
    /// </summary>
    private void ExtractValues(Centroid centroid)
    {
        numericSums.Clear();
        categoricalSums.Clear();

        for (int i = 0; i < centroid.Values.Count; i++)
        {
            var value = centroid.Values[i];

            switch (value)
            {
                case NormalizedNumericValueModel numeric:
                    numericSums[i] = numeric.NormalizedValue;
                    break;

                case NormalizedCategoricalValueModel categorical:
                    categoricalSums[i] = (int[])categorical.OneHotValues.Clone();
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported value type at index {i}: {value?.GetType().Name}");
            }
        }
    }

    /// <summary>
    /// Calculates cumulative sums of parameter values from the merge nodes.
    /// </summary>
    private void CalculateValuesSums(List<DataObjectModel> mergeObjects)
    {
        foreach (var mergeObject in mergeObjects)
        {
            for (int i = 0; i < mergeObject.Values.Count; ++i)
            {
                switch (mergeObject.Values[i])
                {
                    case NormalizedNumericValueModel numericParam when numericSums.ContainsKey(i):
                        numericSums[i] += numericParam.NormalizedValue;
                        break;

                    case NormalizedCategoricalValueModel categoricalParam when categoricalSums.ContainsKey(i):
                        AccumulateCategoricalValues(categoricalSums[i], categoricalParam.OneHotValues);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Applies averaging to the accumulated values and constructs new parameters for the centroid.
    /// </summary>
    private List<ParameterValueModel> ApplyAverages(Centroid centroid, int mergedObjectsCount)
    {
        var newParameterValues = new List<ParameterValueModel>();

        foreach (var pair in numericSums)
        {
            var valueModel = centroid.Values[pair.Key];
            var newValue = pair.Value / mergedObjectsCount;

            newParameterValues.Add(new NormalizedNumericValueModel(newValue, valueModel.Parameter, valueModel.Value));
        }

        foreach (var pair in categoricalSums)
        {
            var valueModel = centroid.Values[pair.Key];
            var newOneHot = new int[pair.Value.Length];

            for (int j = 0; j < newOneHot.Length; j++)
            {
                var avgValue = (double)pair.Value[j] / mergedObjectsCount;

                newOneHot[j] = avgValue >= OneHotThreshold ? 1 : 0;
            }

            newParameterValues.Add(new NormalizedCategoricalValueModel(newOneHot, valueModel.Parameter, valueModel.Value));
        }

        return newParameterValues;
    }

    /// <summary>
    /// Adds the values from a categorical parameter to the corresponding cumulative sum array.
    /// </summary>
    private static void AccumulateCategoricalValues(int[] sumArray, int[] values)
    {
        for (int j = 0; j < values.Length; j++)
        {
            sumArray[j] += values[j];
        }
    }
}
