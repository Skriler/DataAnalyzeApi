using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeApi.Services.Normalizers.Parameters;

public class CategoricalParameterNormalizer : ITypeNormalizer
{
    /// <summary>
    /// List of all unique categories for this parameter.
    /// </summary>
    public List<string> Categories { get; } = new();

    public CategoricalParameterNormalizer(string value)
    {
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

        return new NormalizedCategoricalValueModel(parameterValue, oneHotValues);
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

            if (index < 0)
                continue;

            encoded[index] = 1;
        }

        return encoded;
    }

    private static List<string> GetSplitValues(string values) =>
        values.Split(',')
            .Select(v => v.Trim())
            .Where(v => !string.IsNullOrEmpty(v))
            .ToList();
}
