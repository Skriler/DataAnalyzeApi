using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.Entities;

public class ParameterValue
{
    [Key]
    public int Id { get; set; }

    public string? Value { get; set; }

    public int ParameterId { get; set; }
    public Parameter Parameter { get; set; } = default!;

    public int ObjectId { get; set; }
    public DataObject Object { get; set; } = default!;
}
