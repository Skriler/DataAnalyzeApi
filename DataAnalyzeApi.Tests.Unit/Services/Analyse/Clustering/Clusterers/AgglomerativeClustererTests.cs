using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Clustering.Clusterers;
using DataAnalyzeApi.Tests.Unit.Infrastructure.TestData.Models.TestCases.Clusterers;

namespace DataAnalyzeApi.Tests.Unit.Services.Analyse.Clustering.Clusterers;

public class AgglomerativeClustererTests : BaseClustererTests<AgglomerativeClusterer, AgglomerativeSettings>
{
    public AgglomerativeClustererTests()
        : base((calculator, generator) => new AgglomerativeClusterer(calculator, generator))
    { }

    private static AgglomerativeSettings CreateAgglomerativeSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric,
        double threshold)
    {
        return new AgglomerativeSettings(
            NumericMetric: numericMetric,
            CategoricalMetric: categoricalMetric,
            IncludeParameters: false,
            Threshold: threshold);
    }

    protected override AgglomerativeSettings CreateDefaultSettings(
        NumericDistanceMetricType numericMetric,
        CategoricalDistanceMetricType categoricalMetric)
    {
        const double threshold = 0.2;

        return CreateAgglomerativeSettings(
            numericMetric,
            categoricalMetric,
            threshold);
    }

    [Theory]
    [MemberData(
        nameof(AgglomerativeClustererTestData.GetAgglomerativeClustererTestCases),
        MemberType = typeof(AgglomerativeClustererTestData))]
    public void Cluster_ReturnsExpectedDistance(AgglomerativeClustererTestCase testCase)
    {
        var settings = CreateAgglomerativeSettings(
            default,
            default,
            testCase.Threshold);

        ClustererReturnsExpectedClusters(testCase, settings);
    }
}
