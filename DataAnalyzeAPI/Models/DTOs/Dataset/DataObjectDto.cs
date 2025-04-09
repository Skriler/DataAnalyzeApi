namespace DataAnalyzeAPI.Models.DTOs.Dataset;

public record DataObjectDto(
    long Id,
    string Name,
    Dictionary<string, string> ParameterValues
);
