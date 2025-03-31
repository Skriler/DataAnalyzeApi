using DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;
using DataAnalyzeAPI.Models.Enum;

namespace DataAnalyzeAPI.Services.Normalizers;

public class DatasetNormalizer
{
    private readonly Dictionary<long, ITypeNormalizer> normalizers = new();

    public DatasetDto Normalize(DatasetDto dataset)
    {
        InitializeNormalizers(dataset.Objects);

        return CreateNormalizedDataset(dataset);
    }

    /// <summary>
    /// Initializes normalizers for all values in the dataset.
    /// </summary>
    private void InitializeNormalizers(List<DataObjectDto> objects)
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
    private void AddValueToNormalizer(ParameterValueDto parameterValue)
    {
        if (normalizers.TryGetValue(parameterValue.Parameter.Id, out var normalizer))
        {
            normalizer.AddValue(parameterValue.Value);
            return;
        }

        normalizer = CreateNormalizer(parameterValue);
        normalizers.Add(parameterValue.Parameter.Id, normalizer);
    }

    /// <summary>
    /// Creates a normalized dataset based on the original dataset.
    /// </summary>
    private DatasetDto CreateNormalizedDataset(DatasetDto datasetDto)
    {
        var normalizedObjects = datasetDto.Objects
            .ConvertAll(CreateNormalizedObject);

        return new DatasetDto(
            datasetDto.Id,
            datasetDto.Name,
            datasetDto.Parameters,
            normalizedObjects
            );
    }

    /// <summary>
    /// Creates a normalized object from the original object.
    /// </summary>
    private DataObjectDto CreateNormalizedObject(DataObjectDto obj)
    {
        var normalizedValues = obj.Values
            .ConvertAll(pv => normalizers[pv.Parameter.Id].Normalize(pv))
;
        return new DataObjectDto(
            obj.Id,
            obj.Name,
            normalizedValues
            );
    }

    /// <summary>
    /// Creates a normalizer based on the parameter type.
    /// </summary>
    private static ITypeNormalizer CreateNormalizer(ParameterValueDto parameterValue)
    {
        return parameterValue.Parameter.Type switch
        {
            ParameterType.Numeric => new NumericParameterNormalizer(
                parameterValue.Parameter.Id,
                parameterValue.Value
                ),
            ParameterType.Categorical => new CategoricalParameterNormalizer(
                parameterValue.Parameter.Id,
                parameterValue.Value
                ),
            _ => throw new ArgumentException(
                $"Unsupported parameter type: {parameterValue.Parameter.Type}")
        };
    }
}
