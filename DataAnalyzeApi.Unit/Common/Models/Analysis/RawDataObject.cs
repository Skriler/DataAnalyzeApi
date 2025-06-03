namespace DataAnalyzeApi.Unit.Common.Models.Analysis;

/// <summary>
/// Model of test data object that can be used as raw test data.
/// </summary>
public record RawDataObject
{
    /// <summary>
    /// Raw string values for the object.
    /// </summary>
    public List<string> Values { get; init; } = new();
}
