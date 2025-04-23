using DataAnalyzeAPI.Models.DTOs.Auth;
using DataAnalyzeAPI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly AuthService authService;
    private readonly ILogger<AuthController> logger;

    public AuthController(
        AuthService authService,
        ILogger<AuthController> logger)
    {
        this.authService = authService;
        this.logger = logger;
    }

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="dto">The login data dto</param>
    /// <returns>An action result containing the authentication result or an error message</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var authResult = await authService.LoginAsync(dto);

        if (!authResult.Success)
        {
            logger.LogWarning(authResult.Error);
            return Unauthorized(authResult.Error);
        }

        logger.LogInformation("User {Username} successfully logged in", dto.Username);
        return Ok(authResult);
    }

    /// <summary>
    /// Registers a new user with the provided details.
    /// </summary>
    /// <param name="dto">The registration dto containing user information</param>
    /// <returns>An action result indicating the success or failure of the registration</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await authService.UserExistsAsync(dto.Username))
        {
            logger.LogWarning("Registration attempt with existing username: {Username}", dto.Username);
            return Conflict("User already exists.");
        }

        var result = await authService.RegisterAsync(dto);

        if (!result.Succeeded)
        {
            logger.LogWarning("Failed to create user: {Username}, Errors: {Errors}",
                dto.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        logger.LogInformation("User {Username} successfully registered", dto.Username);
        return Ok("User created successfully.");
    }
}
