using DataAnalyzeAPI.Models.Config;
using DataAnalyzeAPI.Models.DTOs.Auth;
using DataAnalyzeAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.configuration = configuration;
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
        var token = GenerateJwtToken(user, userRoles.First());

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

        // TODO refactor
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
        await userManager.AddToRoleAsync(newUser, "User");

        return Ok("User created successfully.");
    }

    private JwtSecurityToken GenerateJwtToken(ApplicationUser user, string role)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: authClaims,
            expires: DateTime.Now.AddMinutes(jwtConfig.ExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
