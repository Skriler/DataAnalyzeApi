using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.Entities;

public class Dataset
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public List<DataObject> Objects { get; set; } = new();

    public List<Parameter> Parameters { get; set; } = new();
}
