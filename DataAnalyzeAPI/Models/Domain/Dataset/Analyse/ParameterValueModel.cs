namespace DataAnalyzeApi.Models.Domain.Dataset.Analyse;

public record ParameterValueModel(
    string Value,
    ParameterStateModel Parameter
)
{
    public virtual ParameterValueModel DeepClone() => this with { };
}