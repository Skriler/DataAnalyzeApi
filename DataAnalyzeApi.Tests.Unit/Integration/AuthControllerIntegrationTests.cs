using DataAnalyzeApi.Models.DTOs.Auth;
using System.Net;
using System.Net.Http.Json;

namespace DataAnalyzeApi.Tests.Integration;

public class AuthControllerIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Register_WhenValidData_ReturnsSuccess()
    {
        // Arrange
        var dto = CreateValidRegisterDto();

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("User created successfully", result);
    }

    [Fact]
    public async Task Register_WhenInvalidUsername_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterDto("ab", "test@example.com", "Password123", "Password123"); // Username too short

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WhenInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterDto("testuser", "invalid-email", "Password123", "Password123");

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WhenWeakPassword_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterDto("testuser", "test@example.com", "weak", "weak");

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WhenMismatchedPasswords_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterDto("testuser", "test@example.com", "Password123", "DifferentPassword123");

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WhenExistingUser_ReturnsConflict()
    {
        // Arrange
        var dto = CreateValidRegisterDto();
        await client.PostAsJsonAsync("/api/auth/register", dto); // Create user first

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", dto); // Try to create again

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("User already exists", result);
    }

    [Fact]
    public async Task Login_WhenValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var registerDto = CreateValidRegisterDto();
        await client.PostAsJsonAsync("/api/auth/register", registerDto);

        var loginDto = new LoginDto(registerDto.Username, registerDto.Password);

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var authResult = await response.Content.ReadFromJsonAsync<AuthResult>();
        Assert.NotNull(authResult);
        Assert.True(authResult.Success);
        Assert.NotNull(authResult.Token);
    }

    [Fact]
    public async Task Login_WhenInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto("nonexistentuser", "wrongpassword");

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WhenEmptyUsername_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto("", "Password123");

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WhenEmptyPassword_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto("testuser", "");

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // Helper method
    private static RegisterDto CreateValidRegisterDto(string suffix = "")
    {
        return new RegisterDto(
            $"testuser{suffix}{Guid.NewGuid().ToString()[..8]}", // Unique username
            $"test{suffix}@example.com",
            "Password123&&",
            "Password123&&"
        );
    }
}
