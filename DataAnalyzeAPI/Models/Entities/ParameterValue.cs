using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class ParameterValue
{
    [Key]
    [JsonIgnore]
    public long Id { get; set; }

    [StringLength(50, ErrorMessage = "Value cannot exceed 50 characters")]
    public string Value { get; set; } = string.Empty;

    [JsonIgnore] public long ParameterId { get; set; }
    [JsonIgnore] public Parameter Parameter { get; set; } = default!;

    [JsonIgnore] public long ObjectId { get; set; }
    [JsonIgnore] public DataObject Object { get; set; } = default!;
}
