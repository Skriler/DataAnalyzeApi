namespace DataAnalyzeApi.Models.DTOs.Analysis.Clustering.Results;

public record ClusterDto
{
    public string Name { get; set; } = string.Empty;

    public List<DataObjectAnalysisDto> Objects { get; set; } = [];

    public ClusterDto() { }

    public ClusterDto(string name, List<DataObjectAnalysisDto> objects)
    {
        Name = name;
        Objects = objects;
    }
}
