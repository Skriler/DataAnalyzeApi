using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.Entities;

namespace DataAnalyzeApi.Unit.Common.Assertions.Analysis.Entities;

public static class BaseEntityAnalysisMapperAssertions
{
    /// <summary>
    /// Verifies that DataObject matches the expected DataObjectAnalysisDto.
    /// </summary>
    public static void AssertDataObjectEqualDto(
        DataObject dataObject,
        DataObjectAnalysisDto dataObjectAnalysisDto,
        bool includeParameterValues = false)
    {
        Assert.Equal(dataObject.Id, dataObjectAnalysisDto.Id);
        Assert.Equal(dataObject.Name, dataObjectAnalysisDto.Name);

        if (includeParameterValues)
        {
            AssertParameterValuesEqualDto(
                dataObject.Values,
                dataObjectAnalysisDto.ParameterValues);
        }
        else
        {
            Assert.Null(dataObjectAnalysisDto.ParameterValues);
        }
    }

    /// <summary>
    /// Verifies that ParameterValue matches the expected ParameterValues dictionary.
    /// </summary>
    public static void AssertParameterValuesEqualDto(
        List<ParameterValue> values,
        Dictionary<string, string> valuesDto)
    {
        Assert.NotNull(valuesDto);
        Assert.Equal(values.Count, valuesDto.Count);

        foreach (var valueModel in values)
        {
            var paramName = valueModel.Parameter.Name;

            Assert.True(valuesDto.ContainsKey(paramName), $"Missing parameter: {paramName}");
            Assert.Equal(valueModel.Value, valuesDto[paramName]);
        }
    }
}
