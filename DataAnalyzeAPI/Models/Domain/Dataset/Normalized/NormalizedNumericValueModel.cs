using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Dataset.Normalized;

public record NormalizedNumericValueModel : ParameterValueModel
{
    public double NormalizedValue { get; }

    public NormalizedNumericValueModel(
        double normalizedValue,
        ParameterStateModel parameter,
        string value
        ) : base(value, parameter)
    {
        NormalizedValue = normalizedValue;
    }

    public override ParameterValueModel DeepClone()
    {
        return new NormalizedNumericValueModel(
            NormalizedValue,
            Parameter,
            Value
            );
    }
}
