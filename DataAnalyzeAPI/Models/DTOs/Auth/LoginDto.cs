using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Models.DTOs.Auth;

public record LoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
