namespace DataAnalyzeApi.Models.DTOs.Analysis.Similarity.Results;

public record SimilarityAnalysisResultDto : BaseAnalysisResultDto
{
    public List<SimilarityPairDto> Similarities { get; init; } = [];
}
