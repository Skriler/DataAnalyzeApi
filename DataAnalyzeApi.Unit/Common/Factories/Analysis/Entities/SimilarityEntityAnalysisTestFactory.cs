using System.Data;
using AutoFixture;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Unit.Common.Factories.Analysis.Entities;

public class SimilarityEntityAnalysisTestFactory : BaseEntityAnalysisTestFactory
{
    /// <summary>
    /// Creates a SimilarityAnalysisResult entity with test data.
    /// </summary>
    public SimilarityAnalysisResult CreateSimilarityAnalysisResult(int pairsCount = 3)
    {
        var similarities = CreateSimilarityPairList(pairsCount);

        return fixture.Build<SimilarityAnalysisResult>()
            .With(r => r.Id, fixture.Create<short>())
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.SimilarityPairs, similarities)
            .Without(r => r.CreatedAt)
            .Without(r => r.RequestHash)
            .Without(r => r.Dataset)
            .Create();
    }

    /// <summary>
    /// Creates a list of SimilarityPair entities with test data.
    /// </summary>
    public List<SimilarityPair> CreateSimilarityPairList(int pairsCount) =>
        Enumerable.Range(0, pairsCount)
            .Select(i => CreateSimilarityPair(i))
            .ToList();

    /// <summary>
    /// Creates a SimilarityPair entity with test data.
    /// </summary>
    public SimilarityPair CreateSimilarityPair(int id = 0)
    {
        var objectA = CreateDataObject();
        var objectB = CreateDataObject();

        return fixture.Build<SimilarityPair>()
            .With(s => s.Id, id)
            .With(s => s.ObjectA, objectA)
            .With(s => s.ObjectAId, objectA.Id)
            .With(s => s.ObjectB, objectB)
            .With(s => s.ObjectBId, objectB.Id)
            .With(s => s.SimilarityPercentage, fixture.Create<double>() % 1)
            .Without(s => s.SimilarityAnalysisResultId)
            .Without(s => s.SimilarityAnalysisResult)
            .Create();
    }

    /// <summary>
    /// Creates a SimilarityAnalysisResultDto with test data.
    /// </summary>
    public SimilarityAnalysisResultDto CreateSimilarityAnalysisResultDto(int pairsCount = 3)
    {
        var similarityDtos = CreateSimilarityPairDtoList(pairsCount);

        return fixture.Build<SimilarityAnalysisResultDto>()
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.Similarities, similarityDtos)
            .Create();
    }

    /// <summary>
    /// Creates a list of SimilarityPairDto with test data.
    /// </summary>
    public List<SimilarityPairDto> CreateSimilarityPairDtoList(int pairsCount) =>
        Enumerable.Range(0, pairsCount)
            .Select(_ => CreateSimilarityPairDto())
            .ToList();

    /// <summary>
    /// Creates a SimilarityPairDto with test data.
    /// </summary>
    public SimilarityPairDto CreateSimilarityPairDto()
    {
        var objectA = CreateDataObjectAnalysisDto();
        var objectB = CreateDataObjectAnalysisDto();

        return fixture.Build<SimilarityPairDto>()
            .With(s => s.ObjectA, objectA)
            .With(s => s.ObjectB, objectB)
            .With(s => s.SimilarityPercentage, fixture.Create<double>() % 1)
            .Create();
    }
}
