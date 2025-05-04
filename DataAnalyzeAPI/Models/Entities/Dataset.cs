using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class Dataset
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public List<Parameter> Parameters { get; set; } = new();

    [JsonIgnore]
    public List<DataObject> Objects { get; set; } = new();
}
