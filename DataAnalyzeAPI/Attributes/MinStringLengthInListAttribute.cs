using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class MinStringLengthInListAttribute : ValidationAttribute
{
    public int MinLength { get; }

    public MinStringLengthInListAttribute(int minLength)
    {
        MinLength = minLength;
        ErrorMessage = $"Each string in the list must be at least {MinLength} characters long.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IEnumerable<string> list)
            return new ValidationResult($"{validationContext.DisplayName} cannot be null.");

        var invalidItem = list.FirstOrDefault(item => item == null || item.Length < MinLength);

        if (invalidItem != null)
        {
            return new ValidationResult(
                ErrorMessage ?? $"Item {invalidItem} in {validationContext.DisplayName} must be at least {MinLength} characters long."
            );
        }

        return ValidationResult.Success;
    }
}
