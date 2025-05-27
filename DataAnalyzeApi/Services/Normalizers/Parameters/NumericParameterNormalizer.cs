using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeApi.Services.Normalizers.Parameters;

public class NumericParameterNormalizer : ITypeNormalizer
{
    public double Min { get; private set; } = double.MaxValue;

    public double Max { get; private set; } = double.MinValue;

    public double Average => (Min + Max) / 2;

    public NumericParameterNormalizer(string value)
    {
        AddValue(value);
    }

    /// <summary>
    /// Adds a value and updates the min, max, and average values.
    /// </summary>
    public void AddValue(string value)
    {
        if (!double.TryParse(value, out var numericValue))
            throw new ArgumentException($"Invalid numeric value: {value}");

        Min = Math.Min(Min, numericValue);
        Max = Math.Max(Max, numericValue);
    }

    public ParameterValueModel Normalize(ParameterValueModel parameterValue)
    {
        var value = string.IsNullOrEmpty(parameterValue.Value)
            ? Average
            : Convert.ToDouble(parameterValue.Value);

        var normalizedValue = NormalizeMinMax(value);

        return new NormalizedNumericValueModel(parameterValue, normalizedValue);

    }

    /// <summary>
    /// Transforms a value by min-max normalization.
    /// </summary>
    private double NormalizeMinMax(double value)
    {
        if (Max == Min)
            return 1;

        var normalized = (value - Min) / (Max - Min);
        return Math.Clamp(normalized, 0, 1);
    }
}
