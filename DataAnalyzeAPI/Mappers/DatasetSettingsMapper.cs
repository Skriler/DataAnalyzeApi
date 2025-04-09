using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;
using DataAnalyzeAPI.Models.DTOs.Analyse.Settings;
using DataAnalyzeAPI.Models.Entities;
using ParamConfig = DataAnalyzeAPI.Models.Config.ParameterSettingsConfig;

namespace DataAnalyzeAPI.Mappers;

public class DatasetSettingsMapper
{
    /// <summary>
    /// Maps dataset entity to dataset model with parameter settings.
    /// </summary>
    public DatasetModel Map(
        Dataset dataset,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var parameterStates = MapParameterStates(dataset.Parameters, parameterSettings);
        var mappedObjects = MapDataObjects(dataset.Objects, parameterStates);

        return new DatasetModel(
            dataset.Id,
            dataset.Name,
            parameterStates,
            mappedObjects);
    }

    /// <summary>
    /// Maps parameters and parameter settings to their models.
    /// </summary>
    private static List<ParameterStateModel> MapParameterStates(
        List<Parameter> parameters,
        List<ParameterSettingsDto>? parameterSettings)
    {
        var parameterStates = new List<ParameterStateModel>();

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

            var parameterState = new ParameterStateModel(
                parameter.Id,
                parameter.Name,
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
    /// Maps data objects to their models.
    /// </summary>
    private static List<DataObjectModel> MapDataObjects(
        List<DataObject> sourceObjects,
        List<ParameterStateModel> parameterStates)
    {
        var mappedObjects = new List<DataObjectModel>();

        foreach (var sourceObject in sourceObjects)
        {
            var mappedValues = MapParameterValues(sourceObject.Values, parameterStates);

            var mappedObject = new DataObjectModel(
                sourceObject.Id,
                sourceObject.Name,
                mappedValues
                );
            mappedObjects.Add(mappedObject);
        }

        return mappedObjects;
    }

    /// <summary>
    /// Maps parameter values to their models.
    /// </summary>
    private static List<ParameterValueModel> MapParameterValues(
        List<ParameterValue> sourceValues,
        List<ParameterStateModel> parameterStates)
    {
        var mappedValues = new List<ParameterValueModel>();

        foreach (var sourceValue in sourceValues)
        {
            var parameterState = parameterStates
                .First(ps => ps.Id == sourceValue.ParameterId);

            var mappedValue = new ParameterValueModel(
                sourceValue.Value,
                parameterState);
            mappedValues.Add(mappedValue);
        }

        return mappedValues;
    }
}
