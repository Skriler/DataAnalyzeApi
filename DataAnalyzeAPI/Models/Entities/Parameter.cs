using DataAnalyzeApi.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class Parameter
{
    [Key]
    [JsonIgnore]
    public long Id { get; set; }

    [Required(ErrorMessage = "Parameter name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Parameter name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore] public ParameterType Type { get; set; }

    [JsonIgnore] public long DatasetId { get; set; }
    [JsonIgnore] public Dataset Dataset { get; set; } = default!;
}
