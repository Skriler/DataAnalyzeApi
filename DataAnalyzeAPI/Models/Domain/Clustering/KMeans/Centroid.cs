using DataAnalyzeAPI.Models.Config.Clustering;
using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeAPI.Models.Domain.Clustering.KMeans;

public class Centroid
{
    /// <summary>
    /// Threshold used to convert averaged one-hot values to 1 or 0.
    /// If the average is greater than or equal to the threshold, set to 1; otherwise, 0.
    /// </summary>
    private const double OneHotThreshold = KMeansConfig.CentroidConfig.OneHotThreshold;

    public List<ParameterValueModel> Values { get; }

    public int MergedObjectsCount { get; private set; } = 1;

    public Centroid(DataObjectModel obj)
    {
        Values = obj.Values.ConvertAll(v => v.DeepClone());
    }

    /// <summary>
    /// Recalculates the centroid values based on a merged objects list.
    /// </summary>
    public void Recalculate(List<DataObjectModel> mergeObjects)
    {
        if (mergeObjects.Any(obj => obj.Values.Count != Values.Count))
            throw new ArgumentException("Object values count doesn't match centroid's");

        MergedObjectsCount = 1;

        foreach (var mergeObj in mergeObjects)
        {
            for (int i = 0; i < mergeObj.Values.Count; ++i)
            {
                MergeParameterValue(i, mergeObj.Values[i]);
            }

            ++MergedObjectsCount;
        }

        NormalizeCategoricalValues();
    }

    /// <summary>
    /// Merges parameter value from another object into the centroid.
    /// Handles both numeric and categorical parameter types.
    /// </summary>
    private void MergeParameterValue(int index, ParameterValueModel mergeValue)
    {
        var baseValue = Values[index];

        Values[index] = baseValue switch
        {
            NormalizedNumericValueModel numericBase when mergeValue is NormalizedNumericValueModel numericMerge =>
                MergeNumericValues(numericBase, numericMerge),

            NormalizedCategoricalValueModel categoricalBase when mergeValue is NormalizedCategoricalValueModel categoricalMerge =>
                MergeCategoricalValues(categoricalBase, categoricalMerge),

            _ => baseValue
        };
    }

    /// <summary>
    /// Merges two normalized numeric values using weighted averaging.
    /// </summary>
    private NormalizedNumericValueModel MergeNumericValues(
        NormalizedNumericValueModel baseValue,
        NormalizedNumericValueModel mergeValue)
    {
        var weightedSum = baseValue.NormalizedValue * MergedObjectsCount + mergeValue.NormalizedValue;
        var mergedNormalized = weightedSum / (MergedObjectsCount + 1);

        return new NormalizedNumericValueModel(mergedNormalized, baseValue.Parameter, baseValue.Value);
    }

    /// <summary>
    /// Merges two normalized categorical values by summing their one-hot encoded vectors.
    /// </summary>
    private NormalizedCategoricalValueModel MergeCategoricalValues(
        NormalizedCategoricalValueModel baseValue,
        NormalizedCategoricalValueModel mergeValue)
    {
        var mergedOneHot = baseValue.OneHotValues
            .Zip(mergeValue.OneHotValues, (origVal, mergeVal) => origVal + mergeVal)
            .ToArray();

        return new NormalizedCategoricalValueModel(mergedOneHot, baseValue.Parameter, baseValue.Value);
    }

    /// <summary>
    /// Normalizes the one-hot encoded categorical values by averaging
    /// and applying a threshold to determine the final value.
    /// </summary>
    private void NormalizeCategoricalValues()
    {
        foreach (var value in Values)
        {
            if (value is not NormalizedCategoricalValueModel categoricalValue)
                continue;

            for (int i = 0; i < categoricalValue.OneHotValues.Length; ++i)
            {
                var avgValue = (double)categoricalValue.OneHotValues[i] / MergedObjectsCount;

                categoricalValue.OneHotValues[i] = avgValue >= OneHotThreshold ? 1 : 0;
            }
        }
    }
}
