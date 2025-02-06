using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.Entities;

public class DataObject
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public int DatasetId { get; set; }
    public Dataset Dataset { get; set; } = default!;

    public List<ParameterValue> Values { get; set; } = new();
}
