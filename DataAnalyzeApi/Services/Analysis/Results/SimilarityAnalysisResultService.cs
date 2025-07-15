using AutoMapper;
using DataAnalyzeApi.DAL.Repositories;
using DataAnalyzeApi.DAL.Repositories.Analysis;
using DataAnalyzeApi.Mappers.Analysis.Entities;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Services.Analysis.Results;

public class SimilarityAnalysisResultService : BaseAnalysisResultService<SimilarityAnalysisResult, SimilarityAnalysisResultDto>
{
    public SimilarityAnalysisResultService(
        SimlarityEntityAnalysisMapper analysisMapper,
        SimilarityAnalysisResultRepository repository,
        DatasetRepository datasetRepository,
        IMapper mapper)
        : base(analysisMapper, repository, datasetRepository, mapper)
    { }

    /// <summary>
    /// Attaches the actual dataset objects to each similarity pair in the analysis result.
    /// </summary>
    protected override void AttachObjectsToAnalysisResult(
        SimilarityAnalysisResult entity,
        Dictionary<long, DataObject> datasetObjects)
    {
        foreach (var pair in entity.SimilarityPairs)
        {
            if (!datasetObjects.TryGetValue(pair.ObjectAId, out var objectA))
            {
                throw new InvalidOperationException(
                    $"ObjectA with ID {pair.ObjectAId} not found in dataset {entity.DatasetId}");
            }

            if (!datasetObjects.TryGetValue(pair.ObjectBId, out var objectB))
            {
                throw new InvalidOperationException(
                    $"ObjectB with ID {pair.ObjectBId} not found in dataset {entity.DatasetId}");
            }

            pair.ObjectA = objectA;
            pair.ObjectB = objectB;
        }
    }
}
