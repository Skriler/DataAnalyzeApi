using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Dataset.Normalized;
using DataAnalyzeAPI.Services.Analyse.DistanceCalculators.CategoricalMetrics;
using DataAnalyzeAPI.Services.Analyse.DistanceCalculators.NumericMetrics;

namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators;

public class DistanceCalculator : IDistanceCalculator
{
    private readonly INumericDistanceMetric numericDistanceMetric = default!;
    private readonly ICategoricalDistanceMetric categoricalDistanceMetric = default!;

    public double Calculate(DataObjectModel objectA, DataObjectModel objectB)
    {
        if (numericDistanceMetric == null || categoricalDistanceMetric == null)
            throw new InvalidOperationException("DistanceCalculator has not been initialized.");

        if (objectA.Values.Count != objectB.Values.Count)
            throw new ArgumentException("The objects must have the same number of parameters.");

        var numericParametersA = objectA.Values.OfType<NormalizedNumericValueModel>().ToList();
        var numericParametersB = objectB.Values.OfType<NormalizedNumericValueModel>().ToList();
        var categoricalParametersA = objectA.Values.OfType<NormalizedCategoricalValueModel>().ToList();
        var categoricalParametersB = objectB.Values.OfType<NormalizedCategoricalValueModel>().ToList();

        var numericDistance = CalculateNumericDistance(numericParametersA, numericParametersB);
        var categoricalDistance = CalculateCategoricalDistance(categoricalParametersA, categoricalParametersB);

        return CalculateAverageDistance(
            numericDistance,
            numericParametersA.Count,
            categoricalDistance,
            categoricalParametersA.Count
            );
    }

    private double CalculateNumericDistance(
        List<NormalizedNumericValueModel> numericParametersA,
        List<NormalizedNumericValueModel> numericParametersB)
    {
        if (numericParametersA.Count == 0 || numericParametersB.Count == 0)
            return 0;

        var valuesA = numericParametersA.Select(v => v.NormalizedValue).ToArray();
        var valuesB = numericParametersB.Select(v => v.NormalizedValue).ToArray();

        return numericDistanceMetric.Calculate(valuesA, valuesB);
    }

    private double CalculateCategoricalDistance(
        List<NormalizedCategoricalValueModel> categoricalParametersA,
        List<NormalizedCategoricalValueModel> categoricalParametersB)
    {
        if (categoricalParametersA.Count == 0 || categoricalParametersB.Count == 0)
            return 0;

        var totalDistance = 0d;

        for (int i = 0; i < categoricalParametersA.Count; i++)
        {
            totalDistance += categoricalDistanceMetric.Calculate(
                categoricalParametersA[i].OneHotValues,
                categoricalParametersB[i].OneHotValues);
        }

        return totalDistance / categoricalParametersA.Count;
    }

    private double CalculateAverageDistance(
        double numericDistance,
        int numericParametersCount,
        double categoricalDistance,
        int categoricalParametersCount)
    {
        int totalCount = numericParametersCount + categoricalParametersCount;

        if (totalCount == 0)
            return 0;

        double weightedNumericDistance = numericDistance * numericParametersCount;
        double weightedCategoricalDistance = categoricalDistance * categoricalParametersCount;

        return (weightedNumericDistance + weightedCategoricalDistance) / totalCount;
    }
}
