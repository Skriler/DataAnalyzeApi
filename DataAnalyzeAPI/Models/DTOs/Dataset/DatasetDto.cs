using DataAnalyzeAPI.Models.DTOs.Dataset;

namespace DataAnalyzeAPI.Models.DTOs.Create;

public record DatasetDto(
    string Name,
    List<string> Parameters,
    List<DataObjectDTO> Objects
);
