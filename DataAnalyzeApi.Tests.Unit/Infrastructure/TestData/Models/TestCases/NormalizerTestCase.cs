using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.Objects;

namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases;

/// <summary>
/// Test case for DatasetNormalizerTests.
/// </summary>
public class NormalizerTestCase
{
    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<RawDataObject> RawObjects { get; set; } = new();

    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<NormalizedDataObject> NormalizedObjects { get; set; } = new();
}
