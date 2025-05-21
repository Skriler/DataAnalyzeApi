namespace DataAnalyzeApi.Tests.Common.Models;

public class AuthResult
{
    public bool Success { get; set; }

    public string Token { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public string Error { get; set; } = string.Empty;
}
