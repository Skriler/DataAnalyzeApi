using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class ValidIdAttribute : ValidationAttribute
{
    public ValidIdAttribute()
    {
        ErrorMessage = "ID must be greater than 0.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not long id || id <= 0)
        {
            return new ValidationResult(
                ErrorMessage ?? $"Invalid {validationContext.DisplayName}. ID must be greater than 0."
            );
        }

        return ValidationResult.Success;
    }
}
