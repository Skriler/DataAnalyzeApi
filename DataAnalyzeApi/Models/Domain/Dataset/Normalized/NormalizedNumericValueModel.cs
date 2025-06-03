using DataAnalyzeApi.Models.Domain.Dataset.Analysis;

namespace DataAnalyzeApi.Models.Domain.Dataset.Normalized;

public record NormalizedNumericValueModel(
    long Id,
    string Value,
    long ParameterId,
    ParameterStateModel Parameter,
    double NormalizedValue
) : ParameterValueModel(Id, Value, ParameterId, Parameter)
{
    public NormalizedNumericValueModel(ParameterValueModel model, double normalizedValue)
        : this(model.Id, model.Value, model.ParameterId, model.Parameter, normalizedValue)
    { }

    public override ParameterValueModel DeepClone() =>
        new NormalizedNumericValueModel(
            Id,
            Value,
            ParameterId,
            Parameter,
            NormalizedValue);
}
