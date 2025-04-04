using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.Domain.Dataset.Normalized;

public record NormalizedNumericValueModel(
    double NormalizedValue,
    ParameterStateModel Parameter
    ) : ParameterValueModel(
        NormalizedValue.ToString(),
        Parameter
    )
{
    public override ParameterValueModel DeepClone() =>
        new NormalizedNumericValueModel(NormalizedValue, Parameter);
}
