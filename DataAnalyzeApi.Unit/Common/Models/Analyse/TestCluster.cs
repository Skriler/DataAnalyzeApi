namespace DataAnalyzeApi.Unit.Common.Models.Analyse;

/// <summary>
/// Model of test cluster that can be used as test data.
/// </summary>
public record TestCluster
{
    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<NormalizedDataObject> Objects { get; init; } = new();
}
