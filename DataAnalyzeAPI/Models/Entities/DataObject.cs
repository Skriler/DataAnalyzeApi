using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class DataObject
{
    [Key]
    [JsonIgnore]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore] public long DatasetId { get; set; }
    [JsonIgnore] public Dataset Dataset { get; set; } = default!;

    public List<ParameterValue> Values { get; set; } = new();
}
