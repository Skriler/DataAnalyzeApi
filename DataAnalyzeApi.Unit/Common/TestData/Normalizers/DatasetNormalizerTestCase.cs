using DataAnalyzeApi.Unit.Common.Models.Analyse;

namespace DataAnalyzeApi.Unit.Common.TestData.Normalizers;

/// <summary>
/// Test case for DatasetNormalizerTests.
/// </summary>
public record DatasetNormalizerTestCase
{
    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<RawDataObject> RawObjects { get; init; } = new();

    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<NormalizedDataObject> NormalizedObjects { get; init; } = new();
}
