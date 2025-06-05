using DataAnalyzeApi.Models.DTOs.Auth;

namespace DataAnalyzeApi.Integration.Common.Factories;

/// <summary>
/// Factory for creating test instances of RegisterDto and LoginDto.
/// </summary>
public static class AuthRequestFactory
{
    /// <summary>
    /// Creates a RegisterDto with specified or default values.
    /// </summary>
    public static RegisterDto CreateRegister(
        string? suffix = null,
        string? username = null,
        string? email = null,
        string? password = null,
        string? confirmPassword = null)
    {
        var uniqueId = Guid.NewGuid().ToString("N")[..8];

        var user = username ?? $"testuser{suffix}{uniqueId}";
        var userEmail = email ?? $"test{suffix}{uniqueId}@example.com";
        var pwd = password ?? "Password123&&";
        var confirmPwd = confirmPassword ?? pwd;

        return new RegisterDto(user, userEmail, pwd, confirmPwd);
    }

    /// <summary>
    /// Creates a LoginDto with the specified username and password.
    /// </summary>
    public static LoginDto CreateLogin(string username, string password) =>
        new(username, password);
}
