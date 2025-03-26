namespace DataAnalyzeAPI.Models.DTOs.Dataset.Create;

public record DataObjectCreateDto(
    string Name,
    List<string> Values
);
