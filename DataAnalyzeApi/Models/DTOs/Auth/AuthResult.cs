namespace DataAnalyzeApi.Models.DTOs.Auth;

public record AuthResult
{
    public bool Success { get; init; }

    public string Error { get; init; } = string.Empty;

    public string Token { get; init; } = string.Empty;

    public DateTime Expiration { get; init; }

    public string Username { get; init; } = string.Empty;

    public List<string> Roles { get; init; } = [];
}
