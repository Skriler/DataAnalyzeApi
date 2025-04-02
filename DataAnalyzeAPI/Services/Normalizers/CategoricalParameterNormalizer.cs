using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.DTOs.Dataset.Normalized;

namespace DataAnalyzeAPI.Services.Normalizers;

public class CategoricalParameterNormalizer : ITypeNormalizer
{
    public long ParameterId { get; }

    /// <summary>
    /// List of all unique categories for this parameter.
    /// </summary>
    public List<string> Categories { get; } = new();

    public CategoricalParameterNormalizer(long parameterId, string value)
    {
        ParameterId = parameterId;
        AddValue(value);
    }

    /// <summary>
    /// Adds new parameter values to the category list.
    /// </summary>
    public void AddValue(string value)
    {
        var categories = GetSplitValues(value);

        foreach (var category in categories)
        {
            if (Categories.Contains(category))
                continue;

            Categories.Add(category);
        }
    }

    public ParameterValueModel Normalize(ParameterValueModel parameterValue)
    {
        var oneHotValues = ConvertValueToOneHot(parameterValue.Value);

        return new NormalizedCategoricalValueDto(
            oneHotValues,
            parameterValue.Parameter
            );
    }

    /// <summary>
    /// Converts the original parameter values into a one-hot encoded array based on the categories.
    /// </summary>
    private int[] ConvertValueToOneHot(string originalValue)
    {
        var encoded = new int[Categories.Count];

        if (string.IsNullOrEmpty(originalValue))
            return encoded;

        var values = GetSplitValues(originalValue);

        foreach (var value in values)
        {
            var index = Categories.IndexOf(value);
            encoded[index] = 1;
        }

        return encoded;
    }

    private static List<string> GetSplitValues(string values)
    {
        return values.Split(',')
            .Select(v => v.Trim())
            .ToList();
    }
}
