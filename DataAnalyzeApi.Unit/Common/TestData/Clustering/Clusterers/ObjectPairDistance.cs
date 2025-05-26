namespace DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers;

/// <summary>
/// Represents the distance between two objects identified by their indices.
/// </summary>
public record ObjectPairDistance
{
    /// <summary>
    /// Index of the first object.
    /// </summary>
    public long ObjectAIndex { get; init; }

    /// <summary>
    /// Index of the second object.
    /// </summary>
    public long ObjectBIndex { get; init; }

    /// <summary>
    /// Distance between the two objects.
    /// </summary>
    public double Distance { get; init; }
}
