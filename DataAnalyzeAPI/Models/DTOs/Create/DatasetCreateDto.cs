namespace DataAnalyzeAPI.Models.DTOs.Create;

public record DatasetCreateDto(
    string Name,
    List<string> Parameters,
    List<DataObjectCreateDTO> Objects
);
