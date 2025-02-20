namespace DataAnalyzeAPI.Models.DTOs.Create;

public record DataObjectCreateDTO(
    string Name,
    List<string> Values
);
