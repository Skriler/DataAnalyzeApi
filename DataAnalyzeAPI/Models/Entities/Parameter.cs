using DataAnalyzeApi.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class Parameter
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public ParameterType Type { get; set; }
}
