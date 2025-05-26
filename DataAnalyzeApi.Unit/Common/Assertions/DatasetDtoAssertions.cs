using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.Entities;

namespace DataAnalyzeApi.Unit.Common.Assertions;

public static class DatasetDtoAssertions
{
    /// <summary>
    /// Verifies that the Dataset entity matches the DatasetCreateDto
    /// </summary>
    public static void AssertDatasetEqualCreateDto(DatasetCreateDto dto, Dataset dataset)
    {
        Assert.Equal(dto.Name, dataset.Name);
        AssertParametersEqualNameList(dto.Parameters, dataset.Parameters);
        AssertDataObjectsEqualDtoObjects(dto.Objects, dataset.Objects);
    }

    /// <summary>
    /// Verifies that the Parameter entities matches parameter name list
    /// </summary>
    private static void AssertParametersEqualNameList(List<string> parameterNames, List<Parameter> parameters)
    {
        Assert.Equal(parameterNames.Count, parameters.Count);

        for (int i = 0; i < parameterNames.Count; ++i)
        {
            Assert.Equal(parameterNames[i], parameters[i].Name);
        }
    }

    /// <summary>
    /// Verifies that the DataObject list matches the DataObjectDto list
    /// </summary>
    private static void AssertDataObjectsEqualDtoObjects(List<DataObjectCreateDto> dtoObjects, List<DataObject> dataObjects)
    {
        Assert.Equal(dtoObjects.Count, dataObjects.Count);

        for (int i = 0; i < dtoObjects.Count; ++i)
        {
            var dtoObject = dtoObjects[i];
            var dataObject = dataObjects[i];

            Assert.Equal(dtoObject.Name, dataObject.Name);
            AssertValuesEqualValueList(dtoObject.Values, dataObject.Values);
        }
    }

    /// <summary>
    /// Verifies that the ParameterValue list matches value list
    /// </summary>
    private static void AssertValuesEqualValueList(List<string> values, List<ParameterValue> dataValues)
    {
        Assert.Equal(values.Count, dataValues.Count);

        for (int j = 0; j < values.Count; ++j)
        {
            Assert.Equal(values[j], dataValues[j].Value);
        }
    }
}
