using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Models.Domain.Dataset.Normalized;

public record NormalizedCategoricalValueModel : ParameterValueModel
{
    public int[] OneHotValues { get; }

    public NormalizedCategoricalValueModel(
        int[] oneHotValues,
        ParameterStateModel parameter,
        string value
        ) : base(value, parameter)
    {
        OneHotValues = oneHotValues;
    }

    public override ParameterValueModel DeepClone()
    {
        return new NormalizedCategoricalValueModel(
            (int[])OneHotValues.Clone(),
            Parameter,
            Value
            );
    }
}
