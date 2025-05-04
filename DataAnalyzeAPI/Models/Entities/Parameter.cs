using DataAnalyzeApi.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class Parameter
{
    [Key]
    [JsonIgnore]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore] public long TypeId { get; set; }
    [JsonIgnore] public ParameterType Type { get; set; }
}
