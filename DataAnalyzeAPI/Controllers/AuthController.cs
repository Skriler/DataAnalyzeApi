using DataAnalyzeAPI.Models.Config;
using DataAnalyzeAPI.Models.DTOs.Auth;
using DataAnalyzeAPI.Models.Entities;
using DataAnalyzeAPI.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly JwtTokenService jwtTokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        JwtTokenService jwtTokenService)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.Username);

        if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
        {
            return Unauthorized("Invalid username or password");
        }

        var userRoles = await userManager.GetRolesAsync(user);
        var token = jwtTokenService.GenerateToken(user, userRoles);

        var authResult = new AuthResult()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            Username = user.UserName,
            Role = userRoles.First()
        };

        return Ok(authResult);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var existedUser = await userManager.FindByNameAsync(dto.Username);

        if (existedUser != null)
        {
            return Conflict("User already exists.");
        }

        var newUser = new ApplicationUser()
        {
            UserName = dto.Username,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await userManager.CreateAsync(newUser, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await userManager.AddToRoleAsync(newUser, "User");

        return Ok("User created successfully.");
    } 
}
