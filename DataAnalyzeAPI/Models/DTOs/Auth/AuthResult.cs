namespace DataAnalyzeAPI.Models.DTOs.Auth;

public record AuthResult
{
    public string Token { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
