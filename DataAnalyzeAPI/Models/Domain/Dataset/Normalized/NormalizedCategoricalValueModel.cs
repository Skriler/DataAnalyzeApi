using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.Domain.Dataset.Normalized;

public record NormalizedCategoricalValueModel(
    int[] OneHotValues,
    ParameterStateModel Parameter
    ) : ParameterValueModel(
        string.Join(", ", OneHotValues),
        Parameter
    )
{
    public override ParameterValueModel DeepClone() =>
        new NormalizedCategoricalValueModel((int[])OneHotValues.Clone(), Parameter);
}
