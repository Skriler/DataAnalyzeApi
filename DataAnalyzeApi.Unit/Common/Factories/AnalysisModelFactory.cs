using AutoFixture;
using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Similarity;
using DataAnalyzeApi.Unit.Common.Factories.Models;
using DataAnalyzeApi.Unit.Common.Models.Analysis;

namespace DataAnalyzeApi.Unit.Common.Factories;

public class AnalysisModelFactory
{
    private readonly Fixture fixture;
    private readonly DataObjectModelFactory dataObjectModelFactory;

    public AnalysisModelFactory()
    {
        fixture = new Fixture();
        dataObjectModelFactory = new DataObjectModelFactory(fixture);
    }

    /// <summary>
    /// Creates a Cluster with numeric and categorical values.
    /// </summary>
    public Cluster CreateCluster(TestCluster rawCluster)
    {
        var objectsModels = dataObjectModelFactory.CreateNormalizedList(rawCluster.Objects);

        return fixture.Build<Cluster>()
            .With(c => c.Objects, objectsModels)
            .Create();
    }

    /// <summary>
    /// Creates Cluster list with numeric and categorical values.
    /// </summary>
    public List<Cluster> CreateClusterList(List<TestCluster> rawClusters) =>
        rawClusters.ConvertAll(CreateCluster);

    /// <summary>
    /// Creates a SimilarityPair.
    /// </summary>
    public SimilarityPair CreateSimilarityPair() =>
        fixture.Create<SimilarityPair>();

    /// <summary>
    /// Creates a SimilarityPair.
    /// </summary>
    public List<SimilarityPair> CreateSimilarityPairList(int count) =>
        fixture.CreateMany<SimilarityPair>(count).ToList();
}
