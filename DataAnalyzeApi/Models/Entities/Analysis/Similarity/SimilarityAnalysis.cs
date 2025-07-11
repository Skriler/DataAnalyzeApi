using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Entities.Analysis.Similarity;

public class SimilarityAnalysis : BaseAnalysis
{
    public override AnalysisType AnalysisType => AnalysisType.Similarity;

    public List<SimilarityPair> SimilarityPairDtos { get; set; } = [];
}
