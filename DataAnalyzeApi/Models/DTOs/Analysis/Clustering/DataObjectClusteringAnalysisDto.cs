namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering;

public record DataObjectClusteringAnalysisDto : DataObjectAnalysisDto
{
    public double X { get; set; }
    public double Y { get; set; }
}
