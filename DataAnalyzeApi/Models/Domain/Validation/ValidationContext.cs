namespace DataAnalyzeApi.Models.Domain.Validation;

public class ValidationContext
{
    private readonly List<string> errors = [];

    public IReadOnlyList<string> Errors => errors;

    public bool IsValid => errors.Count == 0;

    public void AddError(string error) => errors.Add(error);

    public void AddErrors(IEnumerable<string> newErrors) => errors.AddRange(newErrors);
}
