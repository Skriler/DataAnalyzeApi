namespace DataAnalyzeApi.Models.Domain.Dataset.Analyse;

public record ParameterValueModel(
    long Id,
    string Value,
    long ParameterId,
    ParameterStateModel Parameter
)
{
    public virtual ParameterValueModel DeepClone() => this with { };
}