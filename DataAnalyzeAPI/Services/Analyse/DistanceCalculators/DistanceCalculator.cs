using DataAnalyzeApi.Extensions;
using DataAnalyzeApi.Services.Analyse.Metrics;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Services.Analyse.DistanceCalculators;

public class DistanceCalculator(MetricFactory metricFactory) : IDistanceCalculator
{
    private readonly MetricFactory metricFactory = metricFactory;

    /// <summary>
    /// Calculates the distance between two value vectors.
    /// Returns a value between 0 and 1.
    /// 0 indicates identical, 1 indicates completely different objects.
    /// </summary>
    public double Calculate(
        List<ParameterValueModel> valuesA,
        List<ParameterValueModel> valuesB,
        NumericDistanceMetricType numericMetricType,
        CategoricalDistanceMetricType categoricalMetricType)
    {
        ValidateVectors(valuesA, valuesB);

        var numericParamsA = valuesA.OfParameterType<NormalizedNumericValueModel>();
        var numericParamsB = valuesB.OfParameterType<NormalizedNumericValueModel>();
        var categoricalParamsA = valuesA.OfParameterType<NormalizedCategoricalValueModel>();
        var categoricalParamsB = valuesB.OfParameterType<NormalizedCategoricalValueModel>();

        var numericDistance = CalculateNumericDistance(numericParamsA, numericParamsB, numericMetricType);
        var categoricalDistance = CalculateCategoricalDistance(categoricalParamsA, categoricalParamsB, categoricalMetricType);

        return CalculateAverageDistance(
            numericDistance,
            numericParamsA.Count,
            categoricalDistance,
            categoricalParamsA.Count
            );
    }

    /// <summary>
    /// Validates input vectors.
    /// Throws exceptions if objects have different parameter counts or are empty.
    /// </summary>
    private void ValidateVectors(List<ParameterValueModel> valuesA, List<ParameterValueModel> valuesB)
    {
        if (valuesA.Count != valuesB.Count)
            throw new ArgumentException("The vectors must have the same number of parameters.");

        if (valuesA.Count == 0)
            throw new ArgumentException("The vectors cannot be empty.");
    }

    /// <summary>
    /// Calculates distance between numeric parameters of two vectors.
    /// </summary>
    private double CalculateNumericDistance(
        List<NormalizedNumericValueModel> parametersA,
        List<NormalizedNumericValueModel> parametersB,
        NumericDistanceMetricType metricType)
    {
        if (parametersA.Count == 0 || parametersB.Count == 0)
            return 0;

        var metric = metricFactory.CreateNumericMetric(metricType);
        var valuesA = parametersA.Select(v => v.NormalizedValue).ToArray();
        var valuesB = parametersB.Select(v => v.NormalizedValue).ToArray();

        return metric.Calculate(valuesA, valuesB);
    }

    /// <summary>
    /// Calculates distance between categorical parameters of two vectors.
    /// </summary>
    private double CalculateCategoricalDistance(
        List<NormalizedCategoricalValueModel> parametersA,
        List<NormalizedCategoricalValueModel> parametersB,
        CategoricalDistanceMetricType metricType)
    {
        if (parametersA.Count == 0 || parametersB.Count == 0)
            return 0;

        var metric = metricFactory.CreateCategoricalMetric(metricType);
        var totalDistance = 0d;

        for (int i = 0; i < parametersA.Count; i++)
        {
            totalDistance += metric.Calculate(
                parametersA[i].OneHotValues,
                parametersB[i].OneHotValues);
        }

        return totalDistance / parametersA.Count;
    }

    /// <summary>
    /// Calculates weighted average distance based on numeric and categorical distances
    /// and their respective parameter counts.
    /// </summary>
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

    private static bool AreVectorsEmpty<T>(List<T> a, List<T> b) => a.Count == 0 || b.Count == 0;
    //private static List<T> GetParametersOfType<T>(List<ParameterValueModel> values) where T : ParameterValueModel
    //    => values.OfType<T>().ToList();
}
