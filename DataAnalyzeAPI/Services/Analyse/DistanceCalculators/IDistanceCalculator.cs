using DataAnalyzeApi.Models.Domain.Dataset.Analyse;
using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Services.Analyse.DistanceCalculators;

public interface IDistanceCalculator
{
    double Calculate(
        List<ParameterValueModel> valuesA,
        List<ParameterValueModel> valuesB,
        NumericDistanceMetricType numericMetricType,
        CategoricalDistanceMetricType categoricalMetricType);
}
