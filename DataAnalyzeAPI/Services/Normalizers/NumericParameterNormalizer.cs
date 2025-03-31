using DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;
using DataAnalyzeAPI.Models.DTOs.Dataset.Normalized;

namespace DataAnalyzeAPI.Services.Normalizers;

public class NumericParameterNormalizer : ITypeNormalizer
{
    public long ParameterId { get; }

    public double Min { get; private set; }

    public double Max { get; private set; }

    public double Average { get; private set; }

    long ITypeNormalizer.ParameterId => ParameterId;

    public NumericParameterNormalizer(long parameterId, string value)
    {
        ParameterId = parameterId;
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
        Average = (Min + Max) / 2;
    }

    public ParameterValueDto Normalize(ParameterValueDto parameterValue)
    {
        var value = string.IsNullOrEmpty(parameterValue.Value)
            ? Convert.ToDouble(parameterValue.Value)
            : Average;

        var normalizedValue = NormalizeMinMax(value);

        return new NormalizedNumericValueDto(
            normalizedValue,
            parameterValue.Parameter
            );
    }

    /// <summary>
    /// Transforms a value by min-max normalization.
    /// </summary>
    private double NormalizeMinMax(double value)
    {
        if (Max == Min)
            return 1;

        return (value - Min) / (Max - Min);
    }
}
