using System.Data;
using AutoFixture;
using DataAnalyzeApi.Models.DTOs.Analysis;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;
using DataAnalyzeApi.Models.Entities;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Entities.Analysis.Similarity;

namespace DataAnalyzeApi.Unit.Common.Factories;

public class EntityAnalysisTestFactory
{
    private readonly Fixture fixture = new();

    /// <summary>
    /// Creates a ClusteringAnalysisResult entity with test data.
    /// </summary>
    public ClusteringAnalysisResult CreateClusteringAnalysisResult(int clustersCount = 2, int objectsPerCluster = 3)
    {
        var clusters = CreateClusterList(clustersCount, objectsPerCluster);

        return fixture.Build<ClusteringAnalysisResult>()
            .With(r => r.Id, fixture.Create<short>())
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.Clusters, clusters)
            .Without(r => r.CreatedAt)
            .Without(r => r.RequestHash)
            .Without(r => r.Algorithm)
            .Without(r => r.Dataset)
            .Create();
    }

    /// <summary>
    /// Creates a list of Cluster entities with test data.
    /// </summary>
    public List<Cluster> CreateClusterList(int clustersCount, int objectsPerCluster)
        => Enumerable.Range(0, clustersCount)
            .Select(i => CreateCluster(objectsPerCluster, i))
            .ToList();

    /// <summary>
    /// Creates a Cluster entity with test data.
    /// </summary>
    public Cluster CreateCluster(int objectsPerCluster, int id = 0)
       => fixture.Build<Cluster>()
            .With(c => c.Id, id)
            .With(c => c.Objects, CreateDataObjectList(objectsPerCluster))
            .Without(c => c.ClusteringAnalysisResultId)
            .Without(c => c.ClusteringAnalysisResult)
            .Create();

    /// <summary>
    /// Creates a ClusteringAnalysisResultDto with test data.
    /// </summary>
    public ClusteringAnalysisResultDto CreateClusteringAnalysisResultDto(int clustersCount = 2, int objectsPerCluster = 3)
    {
        var clusterDtos = CreateClusterDtoList(clustersCount, objectsPerCluster);

        return fixture.Build<ClusteringAnalysisResultDto>()
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.Clusters, clusterDtos)
            .Create();
    }

    /// <summary>
    /// Creates a list of ClusterDto with test data.
    /// </summary>
    public List<ClusterDto> CreateClusterDtoList(int clustersCount, int objectsPerCluster)
        => Enumerable.Range(0, clustersCount)
            .Select(_ => CreateClusterDto(objectsPerCluster))
            .ToList();

    /// <summary>
    /// Creates a ClusterDto with test data.
    /// </summary>
    public ClusterDto CreateClusterDto(int objectsPerCluster)
        => fixture.Build<ClusterDto>()
            .With(c => c.Name, fixture.Create<string>()[..10])
            .With(c => c.Objects, CreateDataObjectAnalysisDtoList(objectsPerCluster))
            .Create();

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
    public List<SimilarityPair> CreateSimilarityPairList(int pairsCount)
        => Enumerable.Range(0, pairsCount)
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
    public List<SimilarityPairDto> CreateSimilarityPairDtoList(int pairsCount)
        => Enumerable.Range(0, pairsCount)
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
    private List<DataObject> CreateDataObjectList(int count) =>
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
    private List<DataObjectAnalysisDto> CreateDataObjectAnalysisDtoList(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateDataObjectAnalysisDto())
            .ToList();
}
