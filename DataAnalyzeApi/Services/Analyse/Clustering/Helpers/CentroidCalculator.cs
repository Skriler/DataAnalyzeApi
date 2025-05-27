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
    private readonly Dictionary<long, double> numericSums = new();

    /// <summary>
    /// Stores the cumulative sum of categorical one-hot encoded values by index.
    /// </summary>
    private readonly Dictionary<long, int[]> categoricalSums = new();

    /// <summary>
    /// Recalculates the centroid by averaging its parameters with the provided merge nodes.
    /// </summary>
    public virtual Centroid Recalculate(Centroid centroid, List<DataObjectModel> mergeObjects)
    {
        if (mergeObjects.Count == 0)
            return centroid;

        Validate(centroid, mergeObjects);

        ExtractValues(centroid);
        AccumulateMergeObjectValues(mergeObjects);

        var newValues = ApplyAverages(centroid, mergeObjects.Count + 1);
        return new Centroid(centroid.Id, centroid.Name, newValues);
    }

    /// <summary>
    /// Extracts and stores the initial values from the centroid for further averaging.
    /// </summary>
    private void ExtractValues(Centroid centroid)
    {
        numericSums.Clear();
        categoricalSums.Clear();

        for (int i = 0; i < centroid.Values.Count; ++i)
        {
            var value = centroid.Values[i];

            switch (value)
            {
                case NormalizedNumericValueModel numeric:
                    numericSums[numeric.ParameterId] = numeric.NormalizedValue;
                    break;

                case NormalizedCategoricalValueModel categorical:
                    categoricalSums[categorical.ParameterId] = (int[])categorical.OneHotValues.Clone();
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported value type at index {i}: {value?.GetType().Name}");
            }
       }
    }

    /// <summary>
    /// Accumulates parameter values from all merge objects.
    /// </summary>
    private void AccumulateMergeObjectValues(List<DataObjectModel> mergeObjects)
    {
        foreach (var mergeObject in mergeObjects)
        {
            foreach (var value in mergeObject.Values)
            {
                AddToCumulativeSums(value);
            }
        }
    }

    /// <summary>
    /// Adds a single parameter value to the appropriate cumulative sum.
    /// </summary>
    private void AddToCumulativeSums(ParameterValueModel value)
    {
        switch (value)
        {
            case NormalizedNumericValueModel numeric when numericSums.ContainsKey(numeric.ParameterId):
                numericSums[numeric.ParameterId] += numeric.NormalizedValue;
                break;

            case NormalizedCategoricalValueModel categorical when categoricalSums.ContainsKey(categorical.ParameterId):
                AccumulateCategoricalValues(categoricalSums[categorical.ParameterId], categorical.OneHotValues);
                break;
        }
    }

    /// <summary>
    /// Adds the values from a categorical parameter to the corresponding cumulative sum array.
    /// </summary>
    private static void AccumulateCategoricalValues(int[] sumArray, int[] values)
    {
        for (int j = 0; j < values.Length; ++j)
        {
            sumArray[j] += values[j];
        }
    }

    /// <summary>
    /// Applies averaging to the accumulated values and constructs new parameters for the centroid.
    /// </summary>
    private List<ParameterValueModel> ApplyAverages(Centroid centroid, int mergedObjectsCount)
    {
        var newParameterValues = new List<ParameterValueModel>(centroid.Values.Count);

        foreach (var numericEntry in numericSums)
        {
            var baseValue = GetValueModelByParameterId(centroid, numericEntry.Key);
            var newValue = numericEntry.Value / mergedObjectsCount;

            newParameterValues.Add(new NormalizedNumericValueModel(baseValue, newValue));
        }

        foreach (var categoricalEntry in categoricalSums)
        {
            var baseValue = GetValueModelByParameterId(centroid, categoricalEntry.Key);
            var averaged = AverageOneHot(categoricalEntry.Value, mergedObjectsCount);

            newParameterValues.Add(new NormalizedCategoricalValueModel(baseValue, averaged));
        }

        return newParameterValues;
    }

    /// <summary>
    /// Finds a base parameter value in the centroid by ParameterId.
    /// </summary>
    private static ParameterValueModel GetValueModelByParameterId(Centroid centroid, long parameterId) =>
        centroid.Values.First(v => v.ParameterId == parameterId);

    /// <summary>
    /// Averages one-hot encoded categorical values using thresholding.
    /// </summary>
    private static int[] AverageOneHot(int[] sumArray, int totalCount)
    {
        var result = new int[sumArray.Length];

        for (int i = 0; i < sumArray.Length; ++i)
        {
            double avg = (double)sumArray[i] / totalCount;
            result[i] = avg >= OneHotThreshold ? 1 : 0;
        }

        return result;
    }

    /// <summary>
    /// Validates that all nodes have the same number of parameters as the centroid.
    /// </summary>
    private static void Validate(Centroid centroid, List<DataObjectModel> mergeObjects)
    {
        if (mergeObjects.Any(obj => obj.Values.Count != centroid.Values.Count))
            throw new ArgumentException("Object values count doesn't match centroid's");
    }
}
