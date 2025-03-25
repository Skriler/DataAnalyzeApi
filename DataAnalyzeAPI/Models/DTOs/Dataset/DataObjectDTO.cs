namespace DataAnalyzeAPI.Models.DTOs.Dataset;

public record DataObjectDto(
    string Name,
    List<string> Values
);
