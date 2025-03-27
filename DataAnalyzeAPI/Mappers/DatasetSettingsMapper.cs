using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;
using DataAnalyzeAPI.Models.DTOs.Dataset.Analysis;
using DataAnalyzeAPI.Models.Entities;
using ParamConfig = DataAnalyzeAPI.Models.Config.ParameterSettingsConfig;

namespace DataAnalyzeAPI.Mappers;

public class DatasetSettingsMapper
{
    /// <summary>
    /// Maps dataset to DTO.
    /// </summary>
    public DatasetDto MapObjects(
        Dataset dataset,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var parameterStates = MapParameterStates(dataset.Parameters, parameterSettings);
        var mappedObjects = MapDataObjects(dataset.Objects, parameterStates);

        return new DatasetDto(
            dataset.Id,
            dataset.Name,
            parameterStates,
            mappedObjects);
    }

    /// <summary>
    /// Maps parameters and their settings to Dto objects.
    /// </summary>
    private static List<ParameterStateDto> MapParameterStates(
        List<Parameter> parameters,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var parameterStates = new List<ParameterStateDto>();

        var totalParameterWeight = parameters
            .ConvertAll(p => GetParameterWeight(p, parameterSettings))
            .Sum();

        foreach (var parameter in parameters)
        {
            var parameterSetting = parameterSettings
                ?.FirstOrDefault(ps => ps.ParameterId == parameter.Id);

            parameterSetting ??= new ParameterSettingsDto()
                {
                    ParameterId = parameter.Id,
                };

            var parameterState = new ParameterStateDto(
                parameter.Id,
                parameter.Type,
                parameterSetting.IsActive,
                parameterSetting.Weight / totalParameterWeight
                );
            parameterStates.Add(parameterState);
        }

        return parameterStates;
    }

    /// <summary>
    /// Returns the weight for a parameter, using its specific settings if available
    /// otherwise returning the default weight.
    /// </summary>
    private static double GetParameterWeight(
        Parameter parameter,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var parameterSetting = parameterSettings
            ?.FirstOrDefault(ps => ps.ParameterId == parameter.Id);

        return parameterSetting?.Weight ?? ParamConfig.Weight.Default;
    }

    /// <summary>
    /// Maps data objects to DTO.
    /// </summary>
    private static List<DataObjectDto> MapDataObjects(
        List<DataObject> sourceObjects,
        List<ParameterStateDto> parameterStates)
    {
        var mappedObjects = new List<DataObjectDto>();

        foreach (var sourceObject in sourceObjects)
        {
            var mappedValues = MapParameterValues(sourceObject.Values, parameterStates);

            var mappedObject = new DataObjectDto(
                sourceObject.Id,
                sourceObject.Name,
                mappedValues
                );
            mappedObjects.Add(mappedObject);
        }

        return mappedObjects;
    }

    /// <summary>
    /// Maps parameter values to DTO.
    /// </summary>
    private static List<ParameterValueDto> MapParameterValues(
        List<ParameterValue> sourceValues,
        List<ParameterStateDto> parameterStates)
    {
        var mappedValues = new List<ParameterValueDto>();

        foreach (var sourceValue in sourceValues)
        {
            var parameterState = parameterStates
                .First(ps => ps.Id == sourceValue.ParameterId);

            var mappedValue = new ParameterValueDto(
                sourceValue.Value,
                parameterState);
            mappedValues.Add(mappedValue);
        }

        return mappedValues;
    }
}
