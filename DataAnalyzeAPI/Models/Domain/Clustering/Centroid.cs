using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeAPI.Models.Domain.Clustering;

public class Centroid
{
    private const float MinThreshold = 0.5f;

    public List<ParameterValueModel> Values { get; }

    public int MergedObjectsCount { get; private set; }

    public Centroid(DataObjectModel obj)
    {
        Values = obj.Values.ConvertAll(v => v.DeepClone());
    }

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
    }

    private void MergeParameterValue(int index, ParameterValueModel mergeValue)
    {
        var origValue = Values[index];

        Values[index] = origValue switch
        {
            NormalizedNumericValueModel numericBase when mergeValue is NormalizedNumericValueModel numericMerge =>
                new NormalizedNumericValueModel(
                    CalculateWeightedAverage(numericBase.NormalizedValue, numericMerge.NormalizedValue),
                    numericBase.Parameter
                ),
            NormalizedCategoricalValueModel categoricalBase when mergeValue is NormalizedCategoricalValueModel categoricalMerge =>
                new NormalizedCategoricalValueModel(
                    CalculateCategoricalAverage(categoricalBase.OneHotValues, categoricalMerge.OneHotValues),
                    categoricalBase.Parameter
                ),
            _ => origValue
        };
    }

    private double CalculateWeightedAverage(double origValue, double mergeValue)
    {
        var weightedSum = (origValue * MergedObjectsCount) + mergeValue;
        var totalCount = MergedObjectsCount + 1;
        return weightedSum / totalCount;
    }

    private int[] CalculateCategoricalAverage(int[] origValues, int[] mergeValues)
    {
        return origValues
            .Zip(mergeValues, (origVal, mergeVal) => origVal + mergeVal)
            .Select(val => val / (float)(MergedObjectsCount + 1) > MinThreshold ? 1 : 0)
            .ToArray();
    }
}
