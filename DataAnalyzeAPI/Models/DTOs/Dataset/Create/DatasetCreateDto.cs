namespace DataAnalyzeAPI.Models.DTOs.Dataset.Create;

public record DatasetCreateDto(
    string Name,
    List<string> Parameters,
    List<DataObjectCreateDto> Objects
);
