using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis;

namespace DataAnalyzeApi.Mappers.Analysis.Domain;

/// <summary>
/// Base mapper for converting domain models to analysis DTOs.
/// </summary>
public abstract class BaseDomainAnalysisMapper
{
    /// <summary>
    /// Maps DataObjectModel to DataObjectAnalysisDto.
    /// </summary>
    protected static DataObjectAnalysisDto MapDataObject(
        DataObjectModel dataObject,
        bool includeParameters)
    {
        var parameterValues = MapParameterValues(dataObject.Values, includeParameters);

        return new DataObjectAnalysisDto(
            dataObject.Id,
            dataObject.Name,
            parameterValues!);
    }

    /// <summary>
    /// Maps ParameterValueModel list to dictionary (name, value), optionally including the parameters.
    /// Returns null when includeParameters is false, which will cause the property
    /// to be excluded from JSON serialization when used with JsonIgnore attribute.
    /// </summary>
    protected static Dictionary<string, string>? MapParameterValues(
        List<ParameterValueModel> values,
        bool includeParameters)
    {
        if (!includeParameters)
            return null;

        return values.ToDictionary(
            pv => pv.Parameter.Name,
            pv => pv.Value
        );
    }
}
