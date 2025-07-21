using System.Security.Claims;
using DataAnalyzeApi.Models.DTOs.Auth;
using DataAnalyzeApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController(
    AuthService authService,
    ILogger<AuthController> logger
    ) : ControllerBase
{
    private readonly AuthService authService = authService;
    private readonly ILogger<AuthController> logger = logger;

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="dto">The login data dto</param>
    /// <returns>An action result containing the authentication result or an error message</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthResult>> Login(
        [FromBody] LoginDto dto)
    {
        if (!TryValidateModel(dto, out var errorResult))
        {
            return errorResult!;
        }

        var authResult = await authService.LoginAsync(dto);

        if (!authResult.Success)
        {
            logger.LogWarning("Login failed for user {Username}: {Error}", dto.Username, authResult.Error);
            return Unauthorized(authResult.Error);
        }

        logger.LogInformation("User {Username} successfully logged in", dto.Username);
        return authResult;
    }

    /// <summary>
    /// Registers a new user with the provided details.
    /// </summary>
    /// <param name="dto">The registration dto containing user information</param>
    /// <returns>An action result indicating the success or failure of the registration</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> Register(
        [FromBody] RegisterDto dto)
    {
        if (!TryValidateModel(dto, out var errorResult))
        {
            return errorResult!;
        }

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
        return Created(string.Empty, "User successfully registered");
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    /// <returns>An action result indicating successful logout</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<string> Logout()
    {
        var username = User.Identity?.Name;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        logger.LogInformation("User {Username} (ID: {UserId}) successfully logged out",
            username ?? "Unknown", userId ?? "Unknown");

        return Ok("Successfully logged out");
    }

    /// <summary>
    /// Validates the model state and returns appropriate error response if validation fails.
    /// </summary>
    /// <param name="model">The model to validate</param>
    /// <param name="errorResult">The error result if validation fails</param>
    /// <returns>True if model is valid, false otherwise</returns>
    private bool TryValidateModel<T>(
        T model,
        out ActionResult? errorResult)
    {
        if (model == null)
        {
            errorResult = BadRequest("Request body cannot be null");
            return false;
        }

        if (!ModelState.IsValid)
        {
            errorResult = BadRequest(ModelState);
            return false;
        }

        errorResult = null;
        return true;
    }
}
