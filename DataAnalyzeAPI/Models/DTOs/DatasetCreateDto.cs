namespace DataAnalyzeAPI.Models.DTOs;

public record DatasetCreateDto(
    string Name,
    List<string> Parameters,
    List<DataObjectCreateDTO> Objects
);
