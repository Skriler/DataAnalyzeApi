namespace DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

/// <summary>
/// Represents the distance between two objects identified by their indices.
/// </summary>
public class ObjectPairDistance
{
    /// <summary>
    /// Index of the first object.
    /// </summary>
    public long ObjectAIndex { get; set; }

    /// <summary>
    /// Index of the second object.
    /// </summary>
    public long ObjectBIndex { get; set; }

    /// <summary>
    /// Distance between the two objects.
    /// </summary>
    public double Distance { get; set; }
}
