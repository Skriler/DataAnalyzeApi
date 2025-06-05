using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAnalyzeApi.Models.Entities;

public class Dataset
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = "Dataset name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Dataset name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public List<Parameter> Parameters { get; set; } = [];

    [JsonIgnore]
    public List<DataObject> Objects { get; set; } = [];
}
