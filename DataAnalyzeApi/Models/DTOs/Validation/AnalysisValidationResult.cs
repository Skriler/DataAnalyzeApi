namespace DataAnalyzeApi.Models.DTOs.Validation;

public record AnalysisValidationResult()
{
    public List<string> Errors { get; init; } = [];

    public bool IsValid => Errors.Count == 0;

    public static AnalysisValidationResult Valid() => new();

    public static AnalysisValidationResult Invalid(string error) =>
        new() { Errors = [error] };

    public static AnalysisValidationResult Invalid(List<string> errors) =>
        new() { Errors = errors };
}
