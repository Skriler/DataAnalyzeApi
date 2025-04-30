using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class ParameterValue
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public string Value { get; set; } = string.Empty;

    [JsonIgnore] public int ParameterId { get; set; }
    [JsonIgnore] public Parameter Parameter { get; set; } = default!;

    [JsonIgnore] public int ObjectId { get; set; }
    [JsonIgnore] public DataObject Object { get; set; } = default!;
}
