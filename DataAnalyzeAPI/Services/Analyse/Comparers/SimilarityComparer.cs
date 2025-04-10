using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Similarity;
using DataAnalyzeAPI.Models.Enum;

namespace DataAnalyzeAPI.Services.Analyse.Comparers;

public class SimilarityComparer
{
    private readonly ICompare comparer;

    private readonly Dictionary<long, double> maxRanges = new();

    public SimilarityComparer(ICompare comparer)
    {
        this.comparer = comparer;
    }

    public List<SimilarityPair> CalculateSimilarity(DatasetModel dataset)
    {
        InitializeMaxRanges(dataset.Objects, dataset.Parameters);

        return CompareObjects(dataset.Objects, dataset.Parameters);
    }

    /// <summary>
    /// Initializes the max ranges dictionary for numeric parameters across all objects.
    /// </summary>
    private void InitializeMaxRanges(
        List<DataObjectModel> objects,
        List<ParameterStateModel> parameterStates)
    {
        maxRanges.Clear();

        var numericParameters = parameterStates
            .Where(ps => ps.Type == ParameterType.Numeric)
            .ToList();

        foreach (var parameterState in numericParameters)
        {
            var values = objects
                .SelectMany(obj => obj.Values)
                .Where(v => v.Parameter.Id == parameterState.Id)
                .Select(v => Convert.ToDouble(v.Value))
                .ToList();

            var minValue = values.Min();
            var maxValue = values.Max();

            maxRanges[parameterState.Id] = maxValue > minValue
                ? maxValue - minValue
                : 1;
        }
    }

    /// <summary>
    /// Compares all objects in the dataset and calculates their similarity.
    /// </summary>
    private List<SimilarityPair> CompareObjects(
        List<DataObjectModel> objects,
        List<ParameterStateModel> parameterStates)
    {
        var similarities = new List<SimilarityPair>();

        for (int i = 0; i < objects.Count; ++i)
        {
            var objectA = objects[i];

            for (int j = i + 1; j < objects.Count; ++j)
            {
                var objectB = objects[j];

                var similarity = new SimilarityPair(
                    objectA,
                    objectB,
                    CalculateSimilarityPercentage(objectA, objectB, parameterStates)
                );

                similarities.Add(similarity);
            }
        }

        return similarities;
    }

    /// <summary>
    /// Calculates the weighted similarity percentage between two objects.
    /// </summary>
    private double CalculateSimilarityPercentage(
        DataObjectModel objectA,
        DataObjectModel objectB,
        List<ParameterStateModel> parameterStates)
    {
        if (objectA.Values.Count != objectB.Values.Count)
            throw new InvalidOperationException("Objects must have the same number of parameters.");

        if (maxRanges.Count == 0)
            throw new InvalidOperationException("Max ranges must be initialized.");

        var weightedSimilarity = 0d;
        var totalNormalizedWeight = 0d;

        for (int i = 0; i < parameterStates.Count; ++i)
        {
            var parameterState = parameterStates[i];

            if (!parameterState.IsActive)
                continue;

            var valueA = objectA.Values.First(v => v.Parameter.Id == parameterState.Id).Value;
            var valueB = objectB.Values.First(v => v.Parameter.Id == parameterState.Id).Value;
            var maxRange = maxRanges[parameterState.Id];

            var similarity = comparer.Compare(valueA, valueB, maxRange);

            weightedSimilarity += similarity * parameterState.Weight;
            totalNormalizedWeight += parameterState.Weight;
        }

        return totalNormalizedWeight > 0 ? weightedSimilarity / totalNormalizedWeight : 0;
    }
}
