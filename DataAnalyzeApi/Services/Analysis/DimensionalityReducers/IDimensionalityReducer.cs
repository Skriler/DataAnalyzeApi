using DataAnalyzeApi.Models.Domain.Dataset.Analysis;
using DataAnalyzeApi.Models.Domain.DimensionalityReduction;

namespace DataAnalyzeApi.Services.Analysis.DimensionalityReducers;

public interface IDimensionalityReducer
{
    DimensionalityReductionResult ReduceDimensions(List<DataObjectModel> objects);
}
