namespace DataAnalyzeApi.Tests.Common.Models.Analyse;

/// <summary>
/// Model of test data object that can be used as normalized test data.
/// </summary>
public class NormalizedDataObject
{
    /// <summary>
    /// Normalized numeric values after processing.
    /// </summary>
    public List<double> NumericValues { get; set; } = new();

    /// <summary>
    /// Normalized categorical values after processing (one-hot encoded).
    /// </summary>
    public List<int[]> CategoricalValues { get; set; } = new();
}
