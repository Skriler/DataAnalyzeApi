using DataAnalyzeApi.Exceptions;
using DataAnalyzeApi.Models.Domain.Dataset;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Enum;
using DataAnalyzeApi.Services.Normalizers.Parameters;

namespace DataAnalyzeApi.Services.Normalizers;

public class DatasetNormalizer
{
    private readonly Dictionary<long, ITypeNormalizer> normalizers = new();

    public DatasetModel Normalize(DatasetModel dataset)
    {
        InitializeNormalizers(dataset.Objects);

        return CreateNormalizedDataset(dataset);
    }

    /// <summary>
    /// Initializes normalizers for all values in the dataset.
    /// </summary>
    private void InitializeNormalizers(List<DataObjectModel> objects)
    {
        foreach (var obj in objects)
        {
            foreach (var value in obj.Values)
            {
                AddValueToNormalizer(value);
            }
        }
    }

    /// <summary>
    /// Adds a value to the corresponding normalizer or creates a new one if it doesn't exist.
    /// </summary>
    private void AddValueToNormalizer(ParameterValueModel parameterValue)
    {
        if (normalizers.TryGetValue(parameterValue.ParameterId, out var normalizer))
        {
            normalizer.AddValue(parameterValue.Value);
            return;
        }

        normalizer = CreateNormalizer(parameterValue);
        normalizers.Add(parameterValue.ParameterId, normalizer);
    }

    /// <summary>
    /// Creates a normalized dataset based on the original dataset.
    /// </summary>
    private DatasetModel CreateNormalizedDataset(DatasetModel datasetDto)
    {
        var normalizedObjects = datasetDto.Objects
            .ConvertAll(CreateNormalizedObject);

        return new DatasetModel(
            datasetDto.Id,
            datasetDto.Name,
            datasetDto.Parameters,
            normalizedObjects
            );
    }

    /// <summary>
    /// Creates a normalized object from the original object.
    /// </summary>
    private DataObjectModel CreateNormalizedObject(DataObjectModel obj)
    {
        var normalizedValues = obj.Values
            .ConvertAll(pv => normalizers[pv.ParameterId].Normalize(pv))
;
        return new DataObjectModel(
            obj.Id,
            obj.Name,
            normalizedValues
            );
    }

    /// <summary>
    /// Creates a normalizer based on the parameter type.
    /// </summary>
    private static ITypeNormalizer CreateNormalizer(ParameterValueModel parameterValue)
    {
        return parameterValue.Parameter.Type switch
        {
            ParameterType.Numeric => new NumericParameterNormalizer(parameterValue.Value),
            ParameterType.Categorical => new CategoricalParameterNormalizer(parameterValue.Value),
            _ => throw new TypeNotFoundException(nameof(parameterValue.Parameter.Type))
        };
    }
}
