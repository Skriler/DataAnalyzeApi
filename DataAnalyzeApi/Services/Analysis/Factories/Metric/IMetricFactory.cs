using DataAnalyzeApi.Models.Enums;
using DataAnalyzeApi.Services.Analysis.Metrics;

namespace DataAnalyzeApi.Services.Analysis.Factories.Metric;

public interface IMetricFactory
{
    IDistanceMetric<double> GetNumeric(NumericDistanceMetricType type);

    IDistanceMetric<int> GetCategorical(CategoricalDistanceMetricType type);
}
