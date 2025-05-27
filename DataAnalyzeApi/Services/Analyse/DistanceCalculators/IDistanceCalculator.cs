using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Services.Analyse.DistanceCalculators;

public interface IDistanceCalculator
{
    double Calculate(
        DataObjectModel objectA,
        DataObjectModel objectB,
        NumericDistanceMetricType numericMetricType,
        CategoricalDistanceMetricType categoricalMetricType);
}
