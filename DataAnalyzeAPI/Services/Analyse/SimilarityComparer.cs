using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity;
using DataAnalyzeAPI.Models.DTOs.Dataset.Analysis;
using DataAnalyzeAPI.Models.Enum;

namespace DataAnalyzeAPI.Services.Analyse;

public class SimilarityComparer
{
    private readonly ICompare comparer;

    public SimilarityComparer(ICompare comparer)
    {
        this.comparer = comparer;
    }

    public List<SimilarityPairDto> CalculateSimilarity(
        DatasetDto dataset,
        SimilarityRequest similarityRequest)
    {
        var maxRanges = CalculateMaxRanges(dataset.Objects, dataset.Parameters);

        return CompareObjects(dataset.Objects, maxRanges);
    }

    private static Dictionary<long, double> CalculateMaxRanges(
        List<DataObjectDto> objects,
        List<ParameterStateDto> parameterStates)
    {
        var maxRanges = new Dictionary<long, double>();

        for (int i = 0; i < parameterStates.Count; ++i)
        {
            if (parameterStates[i].Type == ParameterType.Categorical)
                continue;

            var minValue = double.MinValue;
            var maxValue = double.MaxValue;

            foreach(var obj in objects)
            {
                if (!double.TryParse(obj.Values[i].Value, out var number))
                    continue;

                minValue = Math.Min(minValue, number);
                maxValue = Math.Max(maxValue, number);
            }

            maxRanges[i] = maxValue > minValue ? maxValue - minValue : 1;
        }

        return maxRanges;
    }

    private List<SimilarityPairDto> CompareObjects(
        List<DataObjectDto> objects,
        Dictionary<long, double> maxRanges)
    {
        var similarities = new List<SimilarityPairDto>();

        for (int i = 0; i < objects.Count; ++i)
        {
            var objectA = objects[i];

            for (int j = i + 1; j < objects.Count; ++j)
            {
                var objectB = objects[j];

                var similarity = new SimilarityPairDto()
                {
                    ObjectAId = objectA.Id,
                    ObjectAName = objectA.Name,
                    ObjectBId = objectA.Id,
                    ObjectBName = objectB.Name,
                    SimilarityPercentage = GetSimilarityPercentage(objectA, objectB, maxRanges),
                };

                similarities.Add(similarity);
            }
        }

        return similarities;
    }

    private double GetSimilarityPercentage(
        DataObjectDto objectA,
        DataObjectDto objectB,
        Dictionary<long, double> maxRanges)
    {
        if (objectA.Values.Count != objectB.Values.Count)
            throw new InvalidOperationException("Objects must have the same number of parameters");

        var totalWeight = 0d;

        for (int i = 0; i < objectA.Values.Count; ++i)
        {
            var valueA = objectA.Values[i];
            var valueB = objectB.Values[i];
            var maxRange = maxRanges[i];

            var similarity = comparer.Compare(valueA.Value, valueB.Value, maxRange);
            totalWeight += similarity * valueA.Parameter.Weight;
        }

        totalWeight /= objectA.Values.Count;

        return totalWeight;
    }
}
