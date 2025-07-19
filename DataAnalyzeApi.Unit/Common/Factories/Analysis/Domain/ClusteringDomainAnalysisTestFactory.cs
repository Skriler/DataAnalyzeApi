using AutoFixture;
using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.DimensionalityReduction;

namespace DataAnalyzeApi.Unit.Common.Factories.Analysis.Domain;

public class ClusteringDomainAnalysisTestFactory : BaseDomainAnalysisTestFactory
{
    /// <summary>
    /// Creates a ClusterModel with test data.
    /// </summary>
    public ClusterModel CreateClusterModel(int objectsCount = 3)
    {
        var objects = CreateDataObjectModelList(objectsCount);
        return new ClusterModel(fixture.Create<string>()[..10])
        {
            Objects = objects
        };
    }

    /// <summary>
    /// Creates ClusterModel list.
    /// </summary>
    public List<ClusterModel> CreateClusterModelList(int clustersCount, int objectsPerCluster = 3) =>
        Enumerable.Range(0, clustersCount)
            .Select(_ => CreateClusterModel(objectsPerCluster))
            .ToList();

    /// <summary>
    /// Creates DataObjectCoordinateModel list for clusters.
    /// </summary>
    public List<DataObjectCoordinateModel> CreateDataObjectCoordinateModelListForClusters(List<ClusterModel> clusters) =>
        clusters
            .SelectMany(c => c.Objects)
            .Select(obj => CreateDataObjectCoordinateModel(obj.Id))
            .ToList();
}
