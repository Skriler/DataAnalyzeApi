using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;
using DataAnalyzeAPI.Models.DTOs.Dataset.Analysis;
using DataAnalyzeAPI.Models.Entities;

namespace DataAnalyzeAPI.Mappers;

public class DatasetSettingsMapper
{
    public DatasetDto MapObjects(
        Dataset dataset,
        List<ParameterSettingsDto> parameterSettings)
    {
        var parameterStates = MapParameterStates(dataset.Parameters, parameterSettings);
        var mappedObjects = MapDataObjects(dataset.Objects, parameterStates);

        return new DatasetDto(
            dataset.Id,
            dataset.Name,
            parameterStates,
            mappedObjects);
    }

    private static List<ParameterStateDto> MapParameterStates(
        List<Parameter> parameters,
        List<ParameterSettingsDto> parameterSettings)
    {
        var parameterStates = new List<ParameterStateDto>();

        foreach (var parameter in parameters)
        {
            var parameterSetting = parameterSettings
                .FirstOrDefault(ps => ps.ParameterId == parameter.Id);

            parameterSetting ??= new ParameterSettingsDto()
                {
                    ParameterId = parameter.Id,
                };

            var parameterState = new ParameterStateDto(
                parameter.Id,
                parameter.Type,
                parameterSetting.IsActive,
                parameterSetting.Weight
                );
            parameterStates.Add(parameterState);
        }

        return parameterStates;
    }

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

    private static List<ParameterValueDto> MapParameterValues(
        List<ParameterValue> sourceValues,
        List<ParameterStateDto> parameterStates)
    {
        var mappedValues = new List<ParameterValueDto>();

        foreach (var sourceValue in sourceValues)
        {
            var parameterState = parameterStates
                .First(ps => ps.Id == sourceValue.Parameter.Id);

            var mappedValue = new ParameterValueDto(
                sourceValue.Value,
                parameterState);
            mappedValues.Add(mappedValue);
        }

        return mappedValues;
    }
}
