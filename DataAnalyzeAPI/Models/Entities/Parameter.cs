using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeAPI.Models.Entities;

public class Parameter
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}
