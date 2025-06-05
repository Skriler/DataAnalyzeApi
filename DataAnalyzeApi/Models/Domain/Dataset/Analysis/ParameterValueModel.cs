namespace DataAnalyzeApi.Models.Domain.Dataset.Analysis;

public record ParameterValueModel(
    long Id,
    string Value,
    long ParameterId,
    ParameterStateModel Parameter
)
{
    public virtual ParameterValueModel DeepClone() => this with { };
}