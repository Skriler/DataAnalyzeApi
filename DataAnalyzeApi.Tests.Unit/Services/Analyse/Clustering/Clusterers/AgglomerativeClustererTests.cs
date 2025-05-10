using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Clusterers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Clusterers;

public class AgglomerativeClustererTests : BaseClustererTests<AgglomerativeClusterer, AgglomerativeSettings>
{
    protected override AgglomerativeClusterer CreateClusterer(
        IDistanceCalculator calculator,
        ClusterNameGenerator generator)
    {
        return new AgglomerativeClusterer(calculator, generator);
    }

    protected override AgglomerativeSettings CreateSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const double threshold = 0.2;

        return new AgglomerativeSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            Threshold: threshold);
    }

    [Theory]
    [MemberData(nameof(AgglomerativeClustererTestData.GetAgglomerativeClustererTestCases), MemberType = typeof(AgglomerativeClustererTestData))]
    public void Cluster_ReturnsExpectedDistance(AgglomerativeClustererTestCase testCase)
    {
        // Arrange
        var settings = new AgglomerativeSettings(
            NumericMetric: default,
            CategoricalMetric: default,
            IncludeParameters: false,
            Threshold: testCase.Threshold);

        ClustererReturnsExpectedClusters(testCase, settings);
    }
}
