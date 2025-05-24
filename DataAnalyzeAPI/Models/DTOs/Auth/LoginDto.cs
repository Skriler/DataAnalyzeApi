using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Models.DTOs.Auth;

public record LoginDto
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters (a-z, A-Z), numbers, and underscores")]
    public string Username { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
    public string Password { get; init; } = string.Empty;

    public LoginDto(
        string username,
        string password)
    {
        Username = username;
        Password = password;
    }
}
