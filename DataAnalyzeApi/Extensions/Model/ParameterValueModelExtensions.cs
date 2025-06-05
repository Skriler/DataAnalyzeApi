using DataAnalyzeApi.Models.Domain.Dataset.Analysis;

namespace DataAnalyzeApi.Extensions.Model;

public static class ParameterValueModelExtensions
{
    public static List<T> OfParameterType<T>(this IEnumerable<ParameterValueModel> values)
        where T : ParameterValueModel =>
        values
            .OfType<T>()
            .ToList();

    public static List<T> OfParameterTypeOrdered<T>(this IEnumerable<ParameterValueModel> values)
        where T : ParameterValueModel =>
        values
            .OfType<T>()
            .OrderBy(p => p.ParameterId)
            .ToList();
}
