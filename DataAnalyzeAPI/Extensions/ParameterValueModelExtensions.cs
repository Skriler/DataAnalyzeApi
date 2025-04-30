using DataAnalyzeApi.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeApi.Extensions;

public static class ParameterValueModelExtensions
{
    public static List<T> OfParameterType<T>(this IEnumerable<ParameterValueModel> values)
        where T : ParameterValueModel
        => values.OfType<T>().ToList();
}
