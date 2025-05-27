using DataAnalyzeApi.Extensions;
using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Services.Analyse.Factories.Metric;

namespace DataAnalyzeApi.Services.Analyse.DistanceCalculators;

public class DistanceCalculator(IMetricFactory metricFactory) : IDistanceCalculator
{
    private readonly IMetricFactory metricFactory = metricFactory;

    /// <summary>
    /// Calculates the distance between two value vectors.
    /// Returns a value between 0 and 1.
    /// 0 indicates identical, 1 indicates completely different objects.
    /// </summary>
    public double Calculate(
        DataObjectModel objectA,
        DataObjectModel objectB,
        NumericDistanceMetricType numericMetricType,
        CategoricalDistanceMetricType categoricalMetricType)
    {
        ValidateVectors(objectA.Values, objectB.Values);

        var numericParamsA = objectA.Values.OfParameterTypeOrdered<NormalizedNumericValueModel>();
        var numericParamsB = objectB.Values.OfParameterTypeOrdered<NormalizedNumericValueModel>();
        var categoricalParamsA = objectA.Values.OfParameterTypeOrdered<NormalizedCategoricalValueModel>();
        var categoricalParamsB = objectB.Values.OfParameterTypeOrdered<NormalizedCategoricalValueModel>();

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
    /// Calculates distance between numeric parameters of two vectors.
    /// </summary>
    private double CalculateNumericDistance(
        List<NormalizedNumericValueModel> parametersA,
        List<NormalizedNumericValueModel> parametersB,
        NumericDistanceMetricType metricType)
    {
        if (parametersA.Count == 0 || parametersB.Count == 0)
            return 0;

        var metric = metricFactory.GetNumeric(metricType);
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

        var metric = metricFactory.GetCategorical(metricType);
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

    /// <summary>
    /// Validates input vectors.
    /// Throws exceptions if vectors are null, empty or have different lengths.
    /// </summary>
    private static void ValidateVectors(List<ParameterValueModel> valuesA, List<ParameterValueModel> valuesB)
    {
        if (valuesA == null || valuesB == null)
            throw new VectorNullException();

        if (valuesA.Count != valuesB.Count)
            throw new VectorLengthMismatchException();

        if (valuesA.Count == 0)
            throw new EmptyVectorException();
    }
}
