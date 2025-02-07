namespace DataAnalyzeAPI.Models.DTOs;

public record DataObjectCreateDTO(
    string Name,
    List<string> Values
);
