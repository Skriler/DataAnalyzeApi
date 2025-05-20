using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.DTOs.Analyse.Settings;
using DataAnalyzeApi.Models.Entities;

namespace DataAnalyzeApi.Tests.Common.Assertions;

public static class DatasetModelAssertions
{
    /// <summary>
    /// Verifies that the Dataset entity with custom parameter settings matches DatasetModel
    /// </summary>
    public static void AssertDatasetWithSettingsEqualModel(
        Dataset dataset,
        DatasetModel model,
        List<ParameterSettingsDto>? parameterSettings = null)
    {
        Assert.Equal(dataset.Id, model.Id);
        Assert.Equal(dataset.Name, model.Name);
        Assert.Equal(dataset.Parameters.Count, model.Parameters.Count);
        Assert.Equal(dataset.Objects.Count, model.Objects.Count);

        AssertDataObjectsEqualModelObjects(dataset.Objects, model.Objects);

        if (parameterSettings != null)
        {
            AssertParameterSettingsApplied(parameterSettings, model.Parameters);
        }
    }

    /// <summary>
    /// Verifies that DataObject list matches DataObjectModel list
    /// </summary>
    private static void AssertDataObjectsEqualModelObjects(List<DataObject> dataObjects, List<DataObjectModel> modelObjects)
    {
        for (int i = 0; i < dataObjects.Count; ++i)
        {
            var dataObject = dataObjects[i];
            var modelObject = modelObjects[i];

            Assert.Equal(dataObject.Name, modelObject.Name);
            AssertValuesEqualModelValues(dataObject.Values, modelObject.Values);
        }
    }

    /// <summary>
    /// Verifies that ParameterValue list matches ParameterValueModel list
    /// </summary>
    private static void AssertValuesEqualModelValues(List<ParameterValue> dataValues, List<ParameterValueModel> modelValues)
    {
        Assert.Equal(dataValues.Count, modelValues.Count);

        for (int j = 0; j < dataValues.Count; ++j)
        {
            Assert.Equal(dataValues[j].Value, modelValues[j].Value);
        }
    }

    /// <summary>
    /// Verifies that ParameterSettingsDto list are correctly applied to the ParameterStateModel list
    /// </summary>
    private static void AssertParameterSettingsApplied(List<ParameterSettingsDto> settings, List<ParameterStateModel> parameterStates)
    {
        foreach (var setting in settings)
        {
            var modelParameter = parameterStates.FirstOrDefault(p => p.Id == setting.ParameterId);

            Assert.NotNull(modelParameter);
            Assert.Equal(setting.IsActive, modelParameter.IsActive);

            // TODO: Remove weight normalization from mapper
            //Assert.Equal(setting.Weight, modelParameter.Weight, precision: 4);
        }
    }
}
