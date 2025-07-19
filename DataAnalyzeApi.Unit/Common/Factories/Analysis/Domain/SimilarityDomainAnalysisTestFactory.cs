using AutoFixture;
using DataAnalyzeApi.Models.Domain.Similarity;

namespace DataAnalyzeApi.Unit.Common.Factories.Analysis.Domain;

public class SimilarityDomainAnalysisTestFactory : BaseDomainAnalysisTestFactory
{
    /// <summary>
    /// Creates a SimilarityPairModel with test data.
    /// </summary>
    public SimilarityPairModel CreateSimilarityPairModel()
    {
        return new SimilarityPairModel(
            CreateDataObjectModel(),
            CreateDataObjectModel(),
            Math.Abs(fixture.Create<double>()) % 1);
    }

    /// <summary>
    /// Creates SimilarityPairModel list.
    /// </summary>
    public List<SimilarityPairModel> CreateSimilarityPairModelList(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateSimilarityPairModel())
            .ToList();
}
