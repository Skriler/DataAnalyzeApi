using DataAnalyzeApi.Models.Domain.Dataset;
using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeApi.Unit.Common.Assertions;

public static class DatasetAssertions
{
    /// <summary>
    /// Verifies that the DataObjectModel list are equal.
    /// </summary>
    public static void AssertDataObjectsEqual(
           List<DataObjectModel> expected,
           List<DataObjectModel> actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        for (int i = 0; i < expected.Count; ++i)
        {
            AssertParameterValuesEqual(expected[i].Values, actual[i].Values);
        }
    }

    /// <summary>
    /// Verifies that the ParameterValueModel list are equal.
    /// </summary>
    public static void AssertParameterValuesEqual(
           List<ParameterValueModel> expected,
           List<ParameterValueModel> actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        for (int i = 0; i < expected.Count; ++i)
        {
            AssertParameterValueEqual(expected[i], actual[i]);
        }
    }

    /// <summary>
    /// Verifies that pair of ParameterValueModel are equal.
    /// </summary>
    private static void AssertParameterValueEqual(
        ParameterValueModel expected,
        ParameterValueModel actual)
    {
        switch (expected)
        {
            case NormalizedNumericValueModel expectedNumeric when actual is NormalizedNumericValueModel actualNumeric:
                AssertEqualNumericValues(expectedNumeric, actualNumeric);
                return;

            case NormalizedCategoricalValueModel expectedCategorical when actual is NormalizedCategoricalValueModel actualCategorical:
                AssertEqualCategoricalValues(expectedCategorical, actualCategorical);
                return;

            default:
                Assert.Fail($"Unknown parameter type: {expected.GetType().Name}");
                break;
        }
    }

    /// <summary>
    /// Verifies that pair of NormalizedNumericValueModel are equal in normalized value.
    /// </summary>
    private static void AssertEqualNumericValues(
        NormalizedNumericValueModel expected,
        NormalizedNumericValueModel actual)
    {
        Assert.Equal(expected.NormalizedValue, actual.NormalizedValue, precision: 4);
    }

    /// <summary>
    /// Verifies that pair of NormalizedCategoricalValueModel are equal in one hot values.
    /// </summary>
    private static void AssertEqualCategoricalValues(
        NormalizedCategoricalValueModel expected,
        NormalizedCategoricalValueModel actual)
    {
        Assert.Equal(expected.OneHotValues.Length, actual.OneHotValues.Length);

        for (int i = 0; i < expected.OneHotValues.Length; ++i)
        {
            Assert.Equal(expected.OneHotValues[i], actual.OneHotValues[i]);
        }
    }
}
