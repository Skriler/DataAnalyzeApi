using DataAnalyzeApi.Tests.Common.Models.Analyse;

namespace DataAnalyzeApi.Tests.Common.TestData.Clustering.Helpers;

/// <summary>
/// Test case for CentroidCalculatorTests.
/// </summary>
public class CentroidCalculatorTestCase
{
    /// <summary>
    /// Initial centroid data.
    /// </summary>
    public NormalizedDataObject InitialCentroid { get; set; } = new();

    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<NormalizedDataObject> Objects { get; set; } = new();

    /// <summary>
    /// Expected recalculated centroid.
    /// </summary>
    public NormalizedDataObject ExpectedCentroid { get; set; } = new();
}
