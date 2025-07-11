using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Attributes;

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field,
    AllowMultiple = false)]
public class StringLengthRangeInListAttribute : ValidationAttribute
{
    public int MinLength { get; }
    public int MaxLength { get; }

    public StringLengthRangeInListAttribute(int minLength, int maxLength)
    {
        if (minLength < 0)
            throw new ArgumentOutOfRangeException(nameof(minLength), "Minimum length cannot be negative.");

        if (maxLength < minLength)
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be less than minimum length.");

        MinLength = minLength;
        MaxLength = maxLength;

        ErrorMessage = $"Each string in the list must be between {MinLength} and {MaxLength} characters long.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IEnumerable<string> list)
        {
            return new ValidationResult($"{validationContext.DisplayName} must be a list of strings.");
        }

        var invalidItem = list.FirstOrDefault(item =>
            string.IsNullOrEmpty(item) ||
            item.Length < MinLength ||
            item.Length > MaxLength);

        if (invalidItem == null)
            return ValidationResult.Success;

        return new ValidationResult(
            ErrorMessage ??
            $"Item \"{invalidItem}\" in {validationContext.DisplayName} must be between {MinLength} and {MaxLength} characters long."
        );
    }
}
