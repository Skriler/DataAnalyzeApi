namespace DataAnalyzeApi.Models.DTOs.Analysis;

public abstract record BaseAnalysisResultDto
{
    public long DatasetId { get; set; }

    public DateTime CreatedAt { get; set; }
}
