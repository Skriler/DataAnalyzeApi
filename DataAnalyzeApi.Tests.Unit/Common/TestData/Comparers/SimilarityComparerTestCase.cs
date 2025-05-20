using DataAnalyzeApi.Tests.Common.Models;

namespace DataAnalyzeApi.Tests.Common.TestData.Comparers;

public class SimilarityComparerTestCase
{
    /// <summary>
    /// List of objects with their test data.
    /// </summary>
    public List<RawDataObject> Objects { get; set; } = new();

    /// <summary>
    /// Pairwise similarity percentage between values, used for mocking distance calculation.
    /// </summary>
    public List<ParameterValuePairSimilarity> PairwiseSimilarities { get; set; } = new();

    /// <summary>
    /// Expected similarity percentage between object pairs.
    /// </summary>
    public List<ObjectPairSimilarity> ExpectedSimilarities { get; set; } = new();
}
