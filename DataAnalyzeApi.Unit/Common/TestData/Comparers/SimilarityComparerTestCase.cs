using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Common.TestData.Comparers;

public record SimilarityComparerTestCase
{
    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<RawDataObject> Objects { get; init; } = new();

    /// <summary>
    /// Pairwise similarity percentage between values, used for mocking distance calculation.
    /// </summary>
    public List<ParameterValuePairSimilarity> PairwiseSimilarities { get; init; } = new();

    /// <summary>
    /// Expected similarity percentage between object pairs.
    /// </summary>
    public List<ObjectPairSimilarity> ExpectedSimilarities { get; init; } = new();
}
