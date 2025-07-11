using DataAnalyzeApi.Exceptions.Vector;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Models.Enum;

namespace DataAnalyzeApi.Services.Analysis.Comparers;

public class SimilarityComparer(ICompare comparer)
{
    private readonly ICompare comparer = comparer;

    private readonly Dictionary<long, double> maxRanges = [];

    /// <summary>
    /// Calculates similarity pairs between objects in the dataset
    /// based on their parameter values.
    /// </summary>
    public List<SimilarityPairDto> CompareAllObjects(DatasetModel dataset)
    {
        ValidateDataset(dataset);

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
            maxRanges[parameterState.Id] = CalculateRangeForParameter(parameterState, objects);
        }
    }

    /// <summary>
    /// Calculates the value range (max - min) for the given numeric parameter across all provided data objects.
    /// </summary>
    private static double CalculateRangeForParameter(
        ParameterStateModel parameterState,
        List<DataObjectModel> objects)
    {
        double minValue = double.MaxValue;
        double maxValue = double.MinValue;

        foreach (var obj in objects)
        {
            var parameterValue = obj.Values.FirstOrDefault(v => v.ParameterId == parameterState.Id);

            if (parameterValue == null || string.IsNullOrEmpty(parameterValue.Value))
                continue;

            if (!double.TryParse(parameterValue.Value, out double doubleValue))
                continue;

            minValue = Math.Min(minValue, doubleValue);
            maxValue = Math.Max(maxValue, doubleValue);
        }

        return minValue < double.MaxValue && maxValue > minValue
            ? maxValue - minValue
            : 1;
    }

    /// <summary>
    /// Compares all objects in the dataset and calculates their similarity.
    /// </summary>
    private List<SimilarityPairDto> CompareObjects(
        List<DataObjectModel> objects,
        List<ParameterStateModel> parameterStates)
    {
        var activeParameterStates = parameterStates
            .Where(ps => ps.IsActive)
            .ToList();

        var similarities = new List<SimilarityPairDto>();

        for (int i = 0; i < objects.Count; ++i)
        {
            var objectA = objects[i];

            for (int j = i + 1; j < objects.Count; ++j)
            {
                var objectB = objects[j];

                var similarity = new SimilarityPairDto(
                    objectA,
                    objectB,
                    CalculateSimilarityPercentage(objectA, objectB, activeParameterStates)
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
        List<ParameterStateModel> activeParameterStates)
    {
        ValidateVectors(objectA.Values, objectB.Values);

        var weightedSimilarity = 0d;
        var totalNormalizedWeight = 0d;

        foreach (var parameterState in activeParameterStates)
        {
            var parameterValueA = objectA.Values.FirstOrDefault(v => v.ParameterId == parameterState.Id);
            var parameterValueB = objectB.Values.FirstOrDefault(v => v.ParameterId == parameterState.Id);

            if (parameterValueA == null || parameterValueB == null)
                continue;

            var valueA = parameterValueA.Value;
            var valueB = parameterValueB.Value;

            var maxRange = parameterState.Type == ParameterType.Numeric
                ? maxRanges[parameterState.Id]
                : 1;

            var similarity = comparer.Compare(valueA, valueB, maxRange);

            weightedSimilarity += similarity * parameterState.Weight;
            totalNormalizedWeight += parameterState.Weight;
        }

        return totalNormalizedWeight > 0 ? weightedSimilarity / totalNormalizedWeight : 0;
    }

    /// <summary>
    /// Validates input dataset.
    /// Throws exceptions if the dataset is null or if it contains no objects or parameters.
    /// </summary>
    private static void ValidateDataset(DatasetModel dataset)
    {
        ArgumentNullException.ThrowIfNull(dataset);

        if (dataset.Objects == null || dataset.Objects.Count == 0)
            throw new ArgumentException("Dataset must contain objects", nameof(dataset));

        if (dataset.Parameters == null || dataset.Parameters.Count == 0)
            throw new ArgumentException("Dataset must contain parameters", nameof(dataset));
    }

    /// <summary>
    /// Validates input vectors.
    /// Throws exceptions if vectors are null, empty or have different lengths.
    /// </summary>
    private static void ValidateVectors(List<ParameterValueModel> valuesA, List<ParameterValueModel> valuesB)
    {
        if (valuesA == null || valuesB == null)
            throw new VectorNullException();

        if (valuesA.Count == 0)
            throw new EmptyVectorException();

        if (valuesA.Count != valuesB.Count)
            throw new VectorLengthMismatchException();
    }
}
