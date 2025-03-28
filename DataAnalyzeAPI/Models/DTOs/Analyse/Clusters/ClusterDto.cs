using DataAnalyzeAPI.Models.DTOs.Dataset.Analyse;

namespace DataAnalyzeAPI.Models.DTOs.Analyse.Clusters;

public class ClusterDto
{
    public List<DataObjectDto> Objects { get; } = new();
}
