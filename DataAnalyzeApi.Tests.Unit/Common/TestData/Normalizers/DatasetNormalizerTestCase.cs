using DataAnalyzeApi.Tests.Common.Models.Analyse;

namespace DataAnalyzeApi.Tests.Common.TestData.Normalizers;

/// <summary>
/// Test case for DatasetNormalizerTests.
/// </summary>
public class DatasetNormalizerTestCase
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
