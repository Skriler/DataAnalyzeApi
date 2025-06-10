using DataAnalyzeApi.Models.Domain.Validation;
using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using DataAnalyzeApi.Models.DTOs.Validation;

namespace DataAnalyzeApi.Services.Validation;

public class DatasetValidator
{
    private static readonly StringComparer Comparer = StringComparer.OrdinalIgnoreCase;

    private readonly List<Func<DatasetCreateDto, ValidationContext>> validators;

    public DatasetValidator()
    {
        validators =
        [
            ValidateBasicRequirements,
            ValidateObjectValueCounts,
            ValidateParameterTypeConsistency,
        ];
    }

    /// <summary>
    /// Validates the dataset by running multiple validation steps.
    /// </summary>
    public AnalysisValidationResult ValidateDataset(DatasetCreateDto dto)
    {
        if (dto == null)
        {
            return AnalysisValidationResult.Invalid("Dataset cannot be null.");
        }

        var context = new ValidationContext();

        foreach (var validator in validators)
        {
            context = validator(dto);

            if (context.IsValid)
                continue;

            return AnalysisValidationResult.Invalid(context.Errors.ToList());
        }

        return AnalysisValidationResult.Valid();
    }

    /// <summary>
    /// Validates that the dataset meets basic requirements like non-empty parameters and objects,
    /// and checks for duplicate parameter and object names.
    /// </summary>
    private ValidationContext ValidateBasicRequirements(DatasetCreateDto dto)
    {
        var context = new ValidationContext();

        if (dto.Parameters.Count == 0)
            context.AddError("Dataset must contain at least one parameter.");

        if (dto.Objects.Count == 0)
            context.AddError("Dataset must contain at least one object.");

        var duplicateParameters = FindDuplicates(dto.Parameters);
        if (duplicateParameters.Count > 0)
            context.AddError($"Duplicate parameters: {string.Join(", ", duplicateParameters)}");

        var duplicateObjects = FindDuplicates(dto.Objects.Select(o => o.Name));
        if (duplicateObjects.Count > 0)
            context.AddError($"Duplicate object names: {string.Join(", ", duplicateObjects)}");

        return context;
    }

    /// <summary>
    /// Validates that each object in the dataset has the correct number of values,
    /// based on the number of parameters.
    /// </summary>
    private ValidationContext ValidateObjectValueCounts(DatasetCreateDto dto)
    {
        var context = new ValidationContext();

        var parameterCount = dto!.Parameters.Count;

        var invalidObjects = dto.Objects
            .Where(obj => obj.Values.Count != parameterCount)
            .Select(obj => obj.Name)
            .ToList();

        if (invalidObjects.Count > 0)
        {
            context.AddError(
                $"Objects with incorrect value count:\n- {string.Join("\n- ", invalidObjects)}\n" +
                $"Expected: {parameterCount} value(s) per object."
            );
        }

        return context;
    }

    /// <summary>
    /// Validates that the parameter types in the dataset are consistent,
    /// checks that all values for each parameter are of the same type.
    /// </summary>
    private static ValidationContext ValidateParameterTypeConsistency(DatasetCreateDto dto)
    {
        var context = new ValidationContext();

        for (int i = 0; i < dto.Parameters.Count; ++i)
        {
            var parameterName = dto.Parameters[i];
            var values = dto.Objects.ConvertAll(obj => obj.Values[i]);

            ValidateParameterType(context, parameterName, values);
        }

        return context;
    }

    /// <summary>
    /// Validates the type consistency of a single parameter.
    /// </summary>
    private static void ValidateParameterType(
        ValidationContext context,
        string parameterName,
        List<string> values)
    {
        var nonEmptyValues = values
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .ToList();

        if (nonEmptyValues.Count == 0)
            return;

        var (numericValues, categoricalValues) = CategorizeValues(nonEmptyValues);

        if (numericValues.Count == 0 || categoricalValues.Count == 0)
            return;

        var numericSample = string.Join(", ", numericValues.Take(3));
        var categoricalSample = string.Join(", ", categoricalValues.Take(3));

        context.AddError(
            $"Parameter '{parameterName}' has mixed data types. " +
            $"Numeric: [{numericSample}]{(numericValues.Count > 3 ? "..." : "")} " +
            $"Text: [{categoricalSample}]{(categoricalValues.Count > 3 ? "..." : "")}"
        );
    }

    private static (List<string> numeric, List<string> categorical) CategorizeValues(List<string> values)
    {
        var numeric = new List<string>();
        var categorical = new List<string>();

        foreach (var value in values)
        {
            bool isNumeric = double.TryParse(value, out _);

            (isNumeric ? numeric : categorical).Add(value);
        }

        return (numeric, categorical);
    }

    private static List<string> FindDuplicates(IEnumerable<string> items) =>
        items
            .GroupBy(item => item, Comparer)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();
}
