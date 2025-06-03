using DataAnalyzeApi.Models.Domain.Dataset;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Services.Analysis.DistanceCalculators;

public interface IDistanceCalculator
{
    double Calculate(
        DataObjectModel objectA,
        DataObjectModel objectB,
        NumericDistanceMetricType numericMetricType,
        CategoricalDistanceMetricType categoricalMetricType);
}
