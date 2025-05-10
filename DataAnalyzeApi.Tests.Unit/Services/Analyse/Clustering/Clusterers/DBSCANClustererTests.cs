using DataAnalyzeApi.Models.Domain.Clustering;
using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Clusterers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Clusterers;

public class DBSCANClustererTests : BaseClustererTests<DBSCANClusterer, DBSCANSettings>
{
    protected override DBSCANClusterer CreateClusterer(
        IDistanceCalculator calculator,
        ClusterNameGenerator generator)
    {
        return new DBSCANClusterer(calculator, generator);
    }

    protected override DBSCANSettings CreateSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const double epsilon = 0.2;
        const int minPoints = 2;

        return new DBSCANSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            Epsilon: epsilon,
            MinPoints: minPoints);
    }

    [Theory]
    [MemberData(nameof(DBSCANCClustererTestData.GetDBSCANClustererTestCases), MemberType = typeof(DBSCANCClustererTestData))]
    public void Cluster_ReturnsExpectedDistance(DBSCANClustererTestCase testCase)
    {
        var settings = new DBSCANSettings(
            NumericMetric: default,
            CategoricalMetric: default,
            IncludeParameters: false,
            Epsilon: testCase.Epsilon,
            MinPoints: testCase.MinPoints);

        ClustererReturnsExpectedClusters(testCase, settings);
    }

    protected override void AssertClustersEqualsExpected(BaseClustererTestCase testCase, List<Cluster> result)
    {
        base.AssertClustersEqualsExpected(testCase, result);

        if (testCase is DBSCANClustererTestCase dbscanTestCase && dbscanTestCase.ExpectNoiseCluster)
        {
            Assert.Contains(result, c => c.Name.StartsWith("Noise"));
        }
    }
}
