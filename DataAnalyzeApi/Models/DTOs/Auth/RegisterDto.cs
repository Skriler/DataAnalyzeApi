using System.ComponentModel.DataAnnotations;

namespace DataAnalyzeApi.Models.DTOs.Auth;

public record RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters (a-z, A-Z), numbers, and underscores")]
    public string Username { get; init; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(80, ErrorMessage = "Email cannot exceed 80 characters")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match")]
    public string ConfirmPassword { get; init; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters (a-z, A-Z)")]
    public string? FirstName { get; init; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters (a-z, A-Z)")]
    public string? LastName { get; init; }

    public RegisterDto(
        string username,
        string email,
        string password,
        string confirmPassword,
        string? firstName = null,
        string? lastName = null)
    {
        Username = username;
        Email = email;
        Password = password;
        ConfirmPassword = confirmPassword;
        FirstName = firstName;
        LastName = lastName;
    }
}
