using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class DataObject
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore] public int DatasetId { get; set; }
    [JsonIgnore] public Dataset Dataset { get; set; } = default!;

    public List<ParameterValue> Values { get; set; } = new();
}
