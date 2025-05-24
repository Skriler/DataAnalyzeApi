using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Models.DTOs.Dataset.Create;

public record DataObjectCreateDto
{
    [Required(ErrorMessage = "Object name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Object name must be between 3 and 50 characters")]
    public string Name { get; init; }

    [Required(ErrorMessage = "Values are required")]
    [MinLength(1, ErrorMessage = "At least one value is required")]
    public List<string> Values { get; init; }

    public DataObjectCreateDto(
        string name,
        List<string> values)
    {
        Name = name;
        Values = values;
    }
}