using DataAnalyzeApi.Models.Domain.Validation;
using DataAnalyzeApi.Models.DTOs.Dataset.Create;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAnalyzeApi.Services.Validation;

public class DatasetValidator
{
    public DatasetValidationResult ValidateDataset(DatasetCreateDto dto)
    {
        var objectErrors = ValidateObjects(dto);
        var parametersErrors = ValidateParameters(dto.Parameters, dto.Objects);



        return new DatasetValidationResult(errors.Count == 0, errors);
    }

    // Validate that each object has the correct number of values
    private DatasetValidationResult ValidateObjects(DatasetCreateDto dto)
    {
        var errors = new List<string>();

        var parameterCount = dto!.Parameters.Count;

        var invalidObjects = dto.Objects
            .Where(obj => obj.Values?.Count != parameterCount)
            .Select(obj => obj.Name)
            .ToList();

        if (invalidObjects.Count > 0)
        {
            errors.Add(
                $"The following objects have an incorrect number of values:\n" +
                $"- {string.Join("\n- ", invalidObjects)}\n" +
                $"Expected: {parameterCount} value(s) per object."
            );
        }

        return new DatasetValidationResult(errors.Count == 0, errors);
    }

    private DatasetValidationResult ValidateParameters(
        List<string> parameters,
        List<DataObjectCreateDto> objectDtos)
    {
        var errors = new List<string>();

        if (parameters.Count == 0 || objectDtos.Count == 0)
        {
            errors.Add("Dataset must contain at least one parameter and one object.");
        }

        for (int i = 0; i < parameters.Count; ++i)
        {
            var parameterName = parameters[i];
            var values = objectDtos.ConvertAll(obj => obj.Values[i]);

            var isParamValid = ValidateParameterType(parameterName, values);

            if (isParamValid.IsValid)
                continue;

            errors.AddRange(isParamValid.Errors);
        }

        return new DatasetValidationResult(errors.Count == 0, errors);
    }

    private DatasetValidationResult ValidateParameterType(
        string parameterName,
        List<string> values)
    {
        var errors = new List<string>();

        var nonEmptyValues = values
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .ToList();

        if (nonEmptyValues.Count == 0)
        {
            return new DatasetValidationResult(true, errors);
        }

        var numericValues = new List<string>();
        var categoricalValues = new List<string>();

        foreach (var value in nonEmptyValues)
        {
            if (double.TryParse(value, out _))
            {
                numericValues.Add(value);
            }
            else
            {
                categoricalValues.Add(value);
            }
        }

        if (numericValues.Count > 0 && categoricalValues.Count > 0)
        {
            errors.Add($"Parameter '{parameterName}' contains mixed data types. " +
                      $"Numeric values: [{string.Join(", ", numericValues.Take(3))}]{(numericValues.Count > 3 ? "..." : "")} " +
                      $"Text values: [{string.Join(", ", categoricalValues.Take(3))}]{(categoricalValues.Count > 3 ? "..." : "")}");
        }

        return new DatasetValidationResult(errors.Count == 0, errors);
    }
}
