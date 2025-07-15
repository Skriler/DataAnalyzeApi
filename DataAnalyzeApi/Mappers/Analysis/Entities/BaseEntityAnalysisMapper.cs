using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis;

namespace DataAnalyzeApi.Mappers.Analysis.Entities;

/// <summary>
/// Mapper for converting database entities to analysis DTOs.
/// </summary>
public abstract class BaseEntityAnalysisMapper<TEntity, TDto>
    where TEntity : AnalysisResult
    where TDto : BaseAnalysisResultDto
{
    /// <summary>
    /// Maps analysis result entity to its DTO.
    /// </summary>
    public abstract TDto MapAnalysisResult(TEntity result, bool includeParameters = false);

    /// <summary>
    /// Maps analysis result entity list to DTO list.
    /// </summary>
    public List<TDto> MapAnalysisResultList(List<TEntity> results, bool includeParameters = false)
         => results.ConvertAll(r => MapAnalysisResult(r, includeParameters));

    /// <summary>
    /// Maps DataObject entity to its DTO.
    /// </summary>
    protected static DataObjectAnalysisDto MapDataObject(DataObject dataObject, bool includeParameters)
    {
        var parameterValues = MapParameterValues(dataObject.Values, includeParameters);

        return new DataObjectAnalysisDto(
            dataObject.Id,
            dataObject.Name,
            parameterValues!);
    }

    /// <summary>
    /// Maps ParameterValue entity list to a dictionary (name, value), optionally including the parameters.
    /// Returns null when includeParameters is false, which will cause the property
    /// to be excluded from JSON serialization when used with JsonIgnore attribute.
    /// </summary>
    protected static Dictionary<string, string>? MapParameterValues(List<ParameterValue> values, bool includeParameters)
    {
        if (!includeParameters)
            return null;

        return values.ToDictionary(
            pv => pv.Parameter.Name,
            pv => pv.Value
        );
    }
}
