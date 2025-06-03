using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Common.TestData.Clustering.Helpers;

/// <summary>
/// Test case for CentroidCalculatorTests.
/// </summary>
public record CentroidCalculatorTestCase
{
    /// <summary>
    /// Initial centroid data.
    /// </summary>
    public NormalizedDataObject InitialCentroid { get; init; } = new();

    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<NormalizedDataObject> Objects { get; init; } = new();

    /// <summary>
    /// Expected recalculated centroid.
    /// </summary>
    public NormalizedDataObject ExpectedCentroid { get; init; } = new();
}
