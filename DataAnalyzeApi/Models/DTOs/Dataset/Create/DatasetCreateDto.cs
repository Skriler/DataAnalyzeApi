using System.ComponentModel.DataAnnotations;
using DataAnalyzeApi.Attributes;

namespace DataAnalyzeApi.Models.DTOs.Dataset.Create;

public record DatasetCreateDto
{
    [Required(ErrorMessage = "Dataset name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Dataset name must be between 3 and 50 characters")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Parameters are required")]
    [MinLength(1, ErrorMessage = "At least one parameter is required")]
    [StringLengthRangeInList(3, 50, ErrorMessage = "Each parameter must be between 3 and 50 characters long")]
    public List<string> Parameters { get; init; } = [];

    [Required(ErrorMessage = "Objects are required")]
    [MinLength(1, ErrorMessage = "At least one data object is required")]
    public List<DataObjectCreateDto> Objects { get; init; } = [];
}
