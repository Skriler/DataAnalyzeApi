using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Domain.Dataset.Normalized;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestHelpers;

/// <summary>
/// Class for comparing parameter values of different types.
/// </summary>
public static class ParameterValueComparison
{
    /// <summary>
    /// Verifies that the lists of parameter values are equal in value.
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
    /// Verifies that the lists of parameter values are equal in value.
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
    /// Verifies that parameter value are equal in value.
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
    /// Verifies that two normalized numeric values are equal in normalized value.
    /// </summary>
    private static void AssertEqualNumericValues(
        NormalizedNumericValueModel expected,
        NormalizedNumericValueModel actual)
    {
        Assert.Equal(expected.NormalizedValue, actual.NormalizedValue, precision: 4);
    }

    /// <summary>
    /// Verifies that two normalized categorical values are equal in one hot values.
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
