using System.ComponentModel.DataAnnotations;
using DataAnalyzeApi.Models.DTOs.Analysis;

namespace DataAnalyzeApi.Attributes;

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field,
    AllowMultiple = false)]
public class UniqueParameterIdAttribute : ValidationAttribute
{
    public UniqueParameterIdAttribute()
    {
        ErrorMessage = "ParameterId values in the list must be unique.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IEnumerable<ParameterSettingsDto> list)
            return new ValidationResult($"{validationContext.DisplayName} must be a list of ParameterSettingsDto.");

        var duplicates = list
            .GroupBy(x => x.ParameterId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Count != 0)
        {
            var duplicateIds = string.Join(", ", duplicates);

            return new ValidationResult(
                ErrorMessage ?? $"Duplicate ParameterId(s) found: {duplicateIds} in {validationContext.DisplayName}."
            );
        }

        return ValidationResult.Success;
    }
}
