using DataAnalyzeApi.Models.Domain.Settings;
using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analyse.Clustering.Clusterers;
using DataAnalyzeApi.Services.Analyse.Clustering.Helpers;
using DataAnalyzeApi.Services.Analyse.DistanceCalculators;

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

    //TODO: add tests
}
