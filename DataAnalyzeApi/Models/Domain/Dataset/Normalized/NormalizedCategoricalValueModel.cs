using DataAnalyzeApi.Models.Domain.Dataset.Analysis;

namespace DataAnalyzeApi.Models.Domain.Dataset.Normalized;

public record NormalizedCategoricalValueModel(
    long Id,
    string Value,
    long ParameterId,
    ParameterStateModel Parameter,
    int[] OneHotValues
) : ParameterValueModel(Id, Value, ParameterId, Parameter)
{
    public NormalizedCategoricalValueModel(ParameterValueModel model, int[] OneHotValues)
        : this(model.Id, model.Value, model.ParameterId, model.Parameter, OneHotValues)
    { }

    public override ParameterValueModel DeepClone() =>
        new NormalizedCategoricalValueModel(
            Id,
            Value,
            ParameterId,
            Parameter,
            (int[])OneHotValues.Clone());
}
