using AutoFixture;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering;
using DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;
using DataAnalyzeApi.Models.Entities.Analysis.Clustering;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Unit.Common.Factories.Analysis.Entities;

public class ClusteringEntityAnalysisTestFactory : BaseEntityAnalysisTestFactory
{
    /// <summary>
    /// Creates a ClusteringAnalysisResult entity with test data.
    /// </summary>
    public ClusteringAnalysisResult CreateClusteringAnalysisResult(int clustersCount = 2, int objectsPerCluster = 3)
    {
        var clusters = CreateClusterList(clustersCount, objectsPerCluster);
        var objectCoordinates = CreateObjectCoordinatesList(clusters);

        return fixture.Build<ClusteringAnalysisResult>()
            .With(r => r.Id, fixture.Create<short>())
            .With(r => r.DatasetId, fixture.Create<short>())
            .With(r => r.Algorithm, fixture.Create<ClusteringAlgorithm>)
            .With(r => r.Clusters, clusters)
            .With(r => r.ObjectCoordinates, objectCoordinates)
            .Without(r => r.CreatedAt)
            .Without(r => r.RequestHash)
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
    /// Creates a list of DataObjectCoordinate entities based on clusters.
    /// </summary>
    public List<DataObjectCoordinate> CreateObjectCoordinatesList(List<Cluster> clusters)
        => clusters
            .SelectMany(c => c.Objects)
            .Select(o => CreateDataObjectCoordinate(o.Id))
            .ToList();

    /// <summary>
    /// Creates a DataObjectCoordinate entity with test data.
    /// </summary>
    public DataObjectCoordinate CreateDataObjectCoordinate(long objectId)
        => fixture.Build<DataObjectCoordinate>()
            .With(c => c.ObjectId, objectId)
            .With(c => c.X, fixture.Create<double>())
            .With(c => c.Y, fixture.Create<double>())
            .Without(c => c.Id)
            .Without(c => c.Object)
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
            .With(c => c.Objects, CreateDataObjectClusteringAnalysisDtoList(objectsPerCluster))
            .Create();

    /// <summary>
    /// Creates a DataObjectClusteringAnalysisDto.
    /// </summary>
    private DataObjectClusteringAnalysisDto CreateDataObjectClusteringAnalysisDto()
    {
        return fixture.Build<DataObjectClusteringAnalysisDto>()
            .With(d => d.Id, fixture.Create<short>())
            .With(d => d.Name, fixture.Create<string>()[..10])
            .With(d => d.X, fixture.Create<double>())
            .With(d => d.Y, fixture.Create<double>())
            .Without(d => d.ParameterValues)
            .Create();
    }

    /// <summary>
    /// Creates a DataObjectClusteringAnalysisDto list.
    /// </summary>
    private List<DataObjectClusteringAnalysisDto> CreateDataObjectClusteringAnalysisDtoList(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => CreateDataObjectClusteringAnalysisDto())
            .ToList();
}
