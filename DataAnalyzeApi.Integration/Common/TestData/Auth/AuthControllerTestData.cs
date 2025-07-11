using DataAnalyzeApi.Integration.Common.Factories;
using DataAnalyzeApi.Models.DTOs.Auth;

namespace DataAnalyzeApi.Integration.Common.TestData.Auth;

/// <summary>
/// Class with test data for ClusteringControllerIntegrationTests.
/// </summary>
public static class AuthControllerTestData
{
    public static TheoryData<RegisterDto> ValidRegisterRequestTestCases =>
    [
        // Test case 1
        AuthRequestFactory.CreateRegister(),

        // Test case 2
        AuthRequestFactory.CreateRegister(suffix: "admin"),

        // Test case 3
        AuthRequestFactory.CreateRegister(suffix: "user123"),
    ];

    public static TheoryData<RegisterDto> InvalidRegisterRequestTestCases =>
    [
        // Test case 1: Short username
        AuthRequestFactory.CreateRegister(username: "ab"),

        // Test case 2: Invalid email
        AuthRequestFactory.CreateRegister(email: "invalid-email"),

        // Test case 3: Weak password
        AuthRequestFactory.CreateRegister(
            password: "weak",
            confirmPassword: "weak"),

        // Test case 4: Mismatched passwords
        AuthRequestFactory.CreateRegister(
            password: "Password123&&",
            confirmPassword: "DifferentPassword123&&"),
    ];

    public static TheoryData<LoginDto> InvalidLoginRequestTestCases =>
    [
        // Test case 1: Empty username
        AuthRequestFactory.CreateLogin(
            username: "",
            password: "Password123&&"),

        // Test case 2: Empty password
        AuthRequestFactory.CreateLogin(
            username: "testuser",
            password: ""),
    ];
}
