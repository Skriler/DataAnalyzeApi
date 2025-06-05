using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class DataObject
{
    [Key]
    [JsonIgnore]
    public long Id { get; set; }

    [Required(ErrorMessage = "Object name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Object name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore] public long DatasetId { get; set; }
    [JsonIgnore] public Dataset Dataset { get; set; } = default!;

    public List<ParameterValue> Values { get; set; } = [];
}
