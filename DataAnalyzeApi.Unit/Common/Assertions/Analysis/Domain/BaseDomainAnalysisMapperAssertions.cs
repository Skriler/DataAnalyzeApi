using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis;

namespace DataAnalyzeApi.Unit.Common.Assertions.Analysis.Domain;

public static class BaseDomainAnalysisMapperAssertions
{
    /// <summary>
    /// Verifies that DataObjectModel matches the expected DataObjectAnalysisDto.
    /// </summary>
    public static void AssertDataObjectsEqualDto(
        DataObjectModel dataObject,
        DataObjectAnalysisDto result,
        bool includeParameterValues)
    {
        Assert.Equal(dataObject.Id, result.Id);
        Assert.Equal(dataObject.Name, result.Name);

        if (includeParameterValues)
        {
            AssertParameterValuesEqualDto(
                dataObject.Values,
                result.ParameterValues);
        }
        else
        {
            Assert.Null(result.ParameterValues);
        }
    }

    /// <summary>
    /// Verifies that ParameterValueModel matches the expected ParameterValues dictionary.
    /// </summary>
    public static void AssertParameterValuesEqualDto(
        List<ParameterValueModel> valueModels,
        Dictionary<string, string> resultValues)
    {
        Assert.NotNull(resultValues);
        Assert.Equal(valueModels.Count, resultValues.Count);

        foreach (var valueModel in valueModels)
        {
            var paramName = valueModel.Parameter.Name;

            Assert.True(resultValues.ContainsKey(paramName), $"Missing parameter: {paramName}");
            Assert.Equal(valueModel.Value, resultValues[paramName]);
        }
    }
}
