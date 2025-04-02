using DataAnalyzeAPI.Models.Domain.Dataset.Analyse;

namespace DataAnalyzeAPI.Services.Analyse.DistanceCalculators;

public interface IDistanceCalculator
{
    double Calculate(DataObjectModel objectA, DataObjectModel objectB);
}
