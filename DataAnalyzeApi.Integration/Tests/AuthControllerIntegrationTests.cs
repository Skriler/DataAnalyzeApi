using DataAnalyzeApi.Integration.Common.Factories;
using DataAnalyzeApi.Integration.Common.TestData.Auth;
using DataAnalyzeApi.Models.DTOs.Auth;
using System.Net;
using System.Net.Http.Json;

namespace DataAnalyzeApi.Integration.Tests;

public class AuthControllerIntegrationTests : IntegrationTestBase
{
    private readonly string BaseUrl = "/api/auth";

    [Theory]
    [MemberData(nameof(AuthControllerTestData.ValidRegisterRequestTestCases),
        MemberType = typeof(AuthControllerTestData))]
    public async Task Register_WhenValidRequest_ReturnsSuccess(RegisterDto request)
    {
        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/register",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(AuthControllerTestData.InvalidRegisterRequestTestCases),
        MemberType = typeof(AuthControllerTestData))]
    public async Task Register_WhenInvalidRequest_ReturnsBadRequest(RegisterDto request)
    {
        // Act
        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}/register",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WhenExistingUser_ReturnsConflict()
    {
        // Arrange
        var dto = AuthRequestFactory.CreateRegister();

        // Act
        await client.PostAsJsonAsync($"{BaseUrl}/register", dto);
        var response = await client.PostAsJsonAsync($"{BaseUrl}/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Login_WhenValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var registerDto = AuthRequestFactory.CreateRegister();
        var loginDto = AuthRequestFactory.CreateLogin(
            registerDto.Username,
            registerDto.Password);

        // Act
        await client.PostAsJsonAsync($"{BaseUrl}/register", registerDto);
        var response = await client.PostAsJsonAsync($"{BaseUrl}/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var authResult = await response.Content.ReadFromJsonAsync<AuthResult>();
        Assert.NotNull(authResult);
        Assert.True(authResult.Success);
        Assert.NotNull(authResult.Token);
    }

    [Theory]
    [MemberData(nameof(AuthControllerTestData.InvalidLoginRequestTestCases),
        MemberType = typeof(AuthControllerTestData))]
    public async Task Login_WhenInvalidRequest_ReturnsBadRequest(LoginDto request)
    {
        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/login", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WhenInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = AuthRequestFactory.CreateLogin(
            username: "nonexistentuser",
            password: "wrongpassword");

        // Act
        var response = await client.PostAsJsonAsync($"{BaseUrl}/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
