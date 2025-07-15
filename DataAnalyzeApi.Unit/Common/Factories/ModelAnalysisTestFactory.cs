using AutoFixture;
using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Unit.Common.Factories.Models;
using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Common.Factories;

public class ModelAnalysisTestFactory
{
    private readonly Fixture fixture;
    private readonly DataObjectModelFactory dataObjectModelFactory;

    public ModelAnalysisTestFactory()
    {
        fixture = new Fixture();
        dataObjectModelFactory = new DataObjectModelFactory(fixture);
    }

    /// <summary>
    /// Creates a ClusterModel with numeric and categorical values.
    /// </summary>
    public ClusterModel CreateClusterModel(TestCluster rawCluster)
    {
        var objectsModels = dataObjectModelFactory.CreateNormalizedList(rawCluster.Objects);

        return fixture.Build<ClusterModel>()
            .With(c => c.Objects, objectsModels)
            .Create();
    }

    /// <summary>
    /// Creates ClusterModel list with numeric and categorical values.
    /// </summary>
    public List<ClusterModel> CreateClusterModelList(List<TestCluster> rawClusters) =>
        rawClusters.ConvertAll(CreateClusterModel);

    /// <summary>
    /// Creates a SimilarityPairModel.
    /// </summary>
    public SimilarityPairModel CreateSimilarityPairModel() =>
        fixture.Create<SimilarityPairModel>();

    /// <summary>
    /// Creates a SimilarityPairModel list.
    /// </summary>
    public List<SimilarityPairModel> CreateSimilarityPairModelList(int count) =>
        fixture
            .CreateMany<SimilarityPairModel>(count)
            .ToList();
}
