using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Clustering.Clusterers;
using DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers;
using DataAnalyzeApi.Unit.Common.TestData.Clustering.Clusterers.DBSCAN;

namespace DataAnalyzeApi.Unit.Tests.Services.Analysis.Clustering.Clusterers;

[Trait("Category", "Unit")]
[Trait("Component", "Analysis")]
[Trait("SubComponent", "Clustering")]
[Trait("Algorithm", "DBSCAN")]
public class DBSCANClustererTests : BaseClustererTests<DBSCANClusterer, DBSCANSettings>
{
    public DBSCANClustererTests()
        : base((calculator, generator) => new DBSCANClusterer(calculator, generator))
    { }

    private static DBSCANSettings CreateDBSCANSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric,
        double epsilon,
        int minPoints)
    {
        return new DBSCANSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            Epsilon: epsilon,
            MinPoints: minPoints);
    }

    protected override DBSCANSettings CreateDefaultSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const double epsilon = 0.2;
        const int minPoints = 2;

        return CreateDBSCANSettings(
            numericMetric,
            categoricalMetric,
            epsilon,
            minPoints);
    }

    /// <summary>
    /// Additional verify for the presence of a noise cluster.
    /// </summary>
    protected override void AssertClustersEqualsExpected(BaseClustererTestCase testCase, List<ClusterModel> result)
    {
        base.AssertClustersEqualsExpected(testCase, result);

        if (testCase is DBSCANClustererTestCase dbscanTestCase && dbscanTestCase.ExpectNoiseCluster)
        {
            Assert.Contains(result, c => c.Name.StartsWith("Noise"));
        }
    }

    [Theory]
    [MemberData(
        nameof(DBSCANClustererTestData.DBSCANClustererTestCases),
        MemberType = typeof(DBSCANClustererTestData))]
    public void Cluster_ReturnsExpectedDistance(DBSCANClustererTestCase testCase)
    {
        var settings = CreateDBSCANSettings(
            default,
            default,
            testCase.Epsilon,
            testCase.MinPoints);

        ClustererReturnsExpectedClusters(testCase, settings);
    }
}
