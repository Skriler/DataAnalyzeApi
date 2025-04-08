using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.Domain.Dataset.Normalized;
using DataAnalyzeAPI.Models.Enums;
using DataAnalyzeAPI.Services.Analyse.Metrics.Categorical;
using DataAnalyzeAPI.Services.Analyse.Metrics.Numeric;

namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators;

public class DistanceCalculator : IDistanceCalculator
{
    private readonly Dictionary<NumericDistanceMetricType, INumericDistanceMetric> numericMetrics;
    private readonly Dictionary<CategoricalDistanceMetricType, ICategoricalDistanceMetric> categoricalMetrics;

    public DistanceCalculator(
        Dictionary<NumericDistanceMetricType, INumericDistanceMetric> numericMetrics,
        Dictionary<CategoricalDistanceMetricType, ICategoricalDistanceMetric> categoricalMetrics)
    {
        this.numericMetrics = numericMetrics;
        this.categoricalMetrics = categoricalMetrics;
    }

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

        var numericParamsA = GetNumericParameters(valuesA);
        var numericParamsB = GetNumericParameters(valuesB);
        var categoricalParamsA = GetCategoricalParameters(valuesA);
        var categoricalParamsB = GetCategoricalParameters(valuesB);

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

        var valuesA = parametersA.Select(v => v.NormalizedValue).ToArray();
        var valuesB = parametersB.Select(v => v.NormalizedValue).ToArray();

        return numericMetrics[metricType].Calculate(valuesA, valuesB);
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

        var totalDistance = 0d;

        for (int i = 0; i < parametersA.Count; i++)
        {
            totalDistance += categoricalMetrics[metricType].Calculate(
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

    private static List<NormalizedNumericValueModel> GetNumericParameters(List<ParameterValueModel> values)
        => values.OfType<NormalizedNumericValueModel>().ToList();

    private static List<NormalizedCategoricalValueModel> GetCategoricalParameters(List<ParameterValueModel> values)
        => values.OfType<NormalizedCategoricalValueModel>().ToList();
}
