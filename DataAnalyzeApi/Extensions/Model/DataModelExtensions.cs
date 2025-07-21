using DataAnalyzeApi.Models.Domain.Dataset.Analysis;

namespace DataAnalyzeApi.Extensions.Model;

/// <summary>
/// Extension methods for data model filtering and manipulation.
/// </summary>
public static class DataModelExtensions
{
    #region ParameterValueModel Extensions

    /// <summary>
    /// Returns all parameter values of the specified derived type T.
    /// </summary>
    public static List<T> OfParameterType<T>(this IEnumerable<ParameterValueModel> values)
        where T : ParameterValueModel =>
        values
            .OfType<T>()
            .ToList();

    /// <summary>
    /// Returns all parameter values of the specified derived type T,
    /// ordered by their associated ParameterId.
    /// </summary>
    public static List<T> OfParameterTypeOrdered<T>(this IEnumerable<ParameterValueModel> values)
        where T : ParameterValueModel =>
        values
            .OfType<T>()
            .OrderBy(p => p.ParameterId)
            .ToList();

    /// <summary>
    /// Filters parameter values to include only those with active parameters.
    /// </summary>
    public static List<ParameterValueModel> FilterByActiveParameters(this IEnumerable<ParameterValueModel> values) =>
        values
            .Where(v => v.Parameter?.IsActive == true)
            .ToList();

    #endregion

    #region DataObjectModel Extensions

    /// <summary>
    /// Filters data object to include only active parameter values.
    /// </summary>
    public static DataObjectModel FilterByActiveParameters(this DataObjectModel obj) =>
        obj with
        {
            Values = obj.Values.FilterByActiveParameters()
        };

    /// <summary>
    /// Filters all data objects to include only active parameter values.
    /// </summary>
    public static List<DataObjectModel> FilterByActiveParameters(this IEnumerable<DataObjectModel> objects) =>
        objects
            .Select(obj => obj.FilterByActiveParameters())
            .ToList();

    #endregion

    #region DatasetModel Extensions

    /// <summary>
    /// Filters dataset to include only active parameters and their corresponding values.
    /// </summary>
    public static DatasetModel FilterByActiveParameters(this DatasetModel dataset) =>
        dataset with
        {
            Parameters = dataset.Parameters.Where(p => p.IsActive).ToList(),
            Objects = dataset.Objects.FilterByActiveParameters()
        };

    #endregion
}
