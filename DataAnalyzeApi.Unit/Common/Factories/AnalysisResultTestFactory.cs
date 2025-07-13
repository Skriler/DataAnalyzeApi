using System.Data;
using AutoFixture;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Unit.Common.Factories;

public class AnalysisResultTestFactory
{
    private readonly Fixture fixture = new();

    /// <summary>
    /// Creates a ClusterAnalysisResult entity with test data.
    /// </summary>
    public ClusterAnalysisResult CreateClusterAnalysisResult(int clustersCount = 2, int objectsPerCluster = 3)
    {
        var clusters = new List<Cluster>();

        for (int i = 0; i < clustersCount; ++i)
        {
            var cluster = fixture.Build<Cluster>()
                .With(c => c.Id, i)
                .With(c => c.Objects, CreateDataObjects(objectsPerCluster))
                .Without(c => c.ClusterAnalysisResultId)
                .Without(c => c.ClusterAnalysisResult)
                .Create();

            clusters.Add(cluster);
        }

        return fixture.Build<ClusterAnalysisResult>()
            .With(r => r.Id, fixture.Create<short>())
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.Clusters, clusters)
            .Without(r => r.CreatedAt)
            .Without(r => r.RequestHash)
            .Without(r => r.IncludeParameters)
            .Without(r => r.Algorithm)
            .Without(r => r.Dataset)
            .Create();
    }

    /// <summary>
    /// Creates a ClusterAnalysisResultDto with test data.
    /// </summary>
    public ClusterAnalysisResultDto CreateClusterAnalysisResultDto(int clustersCount = 2, int objectsPerCluster = 3)
    {
        var clusterDtos = new List<ClusterDto>();

        for (int i = 0; i < clustersCount; ++i)
        {
            var clusterDto = fixture.Build<ClusterDto>()
                .With(c => c.Name, fixture.Create<string>()[..10])
                .With(c => c.Objects, CreateDataObjectAnalysisDtos(objectsPerCluster))
                .Create();

            clusterDtos.Add(clusterDto);
        }

        return fixture.Build<ClusterAnalysisResultDto>()
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.Clusters, clusterDtos)
            .Create();
    }

    /// <summary>
    /// Creates a SimilarityAnalysisResult entity with test data.
    /// </summary>
    public SimilarityAnalysisResult CreateSimilarityAnalysisResult(int pairsCount = 3)
    {
        var similarities = new List<SimilarityPair>();

        for (int i = 0; i < pairsCount; ++i)
        {
            var objectA = CreateDataObject();
            var objectB = CreateDataObject();

            var similarity = fixture.Build<SimilarityPair>()
                .With(s => s.Id, i)
                .With(s => s.ObjectA, objectA)
                .With(s => s.ObjectAId, objectA.Id)
                .With(s => s.ObjectB, objectB)
                .With(s => s.ObjectBId, objectB.Id)
                .With(s => s.SimilarityPercentage, fixture.Create<double>() % 1)
                .Without(s => s.SimilarityAnalysisResultId)
                .Without(s => s.SimilarityAnalysisResult)
                .Create();

            similarities.Add(similarity);
        }

        return fixture.Build<SimilarityAnalysisResult>()
            .With(r => r.Id, fixture.Create<short>())
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.SimilarityPairs, similarities)
            .Without(r => r.CreatedAt)
            .Without(r => r.RequestHash)
            .Without(r => r.IncludeParameters)
            .Without(r => r.Dataset)
            .Create();
    }

    /// <summary>
    /// Creates a SimilarityAnalysisResultDto with test data.
    /// </summary>
    public SimilarityAnalysisResultDto CreateSimilarityAnalysisResultDto(int pairsCount = 3)
    {
        var similarityDtos = new List<SimilarityPairDto>();

        for (int i = 0; i < pairsCount; ++i)
        {
            var objectA = CreateDataObjectAnalysisDto();
            var objectB = CreateDataObjectAnalysisDto();

            var similarityDto = fixture.Build<SimilarityPairDto>()
                .With(s => s.ObjectA, objectA)
                .With(s => s.ObjectB, objectB)
                .With(s => s.SimilarityPercentage, fixture.Create<double>() % 1)
                .Create();

            similarityDtos.Add(similarityDto);
        }

        return fixture.Build<SimilarityAnalysisResultDto>()
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.Similarities, similarityDtos)
            .Create();
    }

    /// <summary>
    /// Creates a DataObject entity.
    /// </summary>
    private DataObject CreateDataObject()
    {
        return fixture.Build<DataObject>()
            .With(d => d.Id, fixture.Create<short>())
            .With(d => d.Name, fixture.Create<string>()[..10])
            .Without(d => d.DatasetId)
            .Without(d => d.Dataset)
            .Without(d => d.Values)
            .Create();
    }

    /// <summary>
    /// Creates a DataObject entity list.
    /// </summary>
    private List<DataObject> CreateDataObjects(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateDataObject())
            .ToList();

    /// <summary>
    /// Creates a DataObjectAnalysisDto.
    /// </summary>
    private DataObjectAnalysisDto CreateDataObjectAnalysisDto()
    {
        return fixture.Build<DataObjectAnalysisDto>()
            .With(d => d.Id, fixture.Create<short>())
            .With(d => d.Name, fixture.Create<string>()[..10])
            .Without(d => d.ParameterValues)
            .Create();
    }


    /// <summary>
    /// Creates a DataObjectAnalysisDto list.
    /// </summary>
    private List<DataObjectAnalysisDto> CreateDataObjectAnalysisDtos(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateDataObjectAnalysisDto())
            .ToList();
}
