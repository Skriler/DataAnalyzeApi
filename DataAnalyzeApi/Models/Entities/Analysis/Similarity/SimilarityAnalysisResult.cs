using Microsoft.EntityFrameworkCore;

namespace DataAnalyzeApi.Models.Entities.Analysis.Similarity;

[Index(nameof(DatasetId), Name = "IX_SimilarityAnalysisResults_DatasetId")]
[Index(nameof(RequestHash), Name = "IX_SimilarityAnalysisResults_RequestHash")]
public class SimilarityAnalysisResult : AnalysisResult
{
    public List<SimilarityPair> SimilarityPairs { get; set; } = [];
}
