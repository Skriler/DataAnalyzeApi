namespace DataAnalyzeApi.Models.Domain.Validation;

public class DatasetValidationResult(bool isValid, List<string> errors)
{
    public bool IsValid { get; } = isValid;

    public List<string> Errors { get; } = errors ?? new();
}
