using DataAnalyzeApi.Models.Config.Identity;
using DataAnalyzeApi.Models.DTOs.Auth;
using DataAnalyzeApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace DataAnalyzeApi.Services.Auth;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    JwtTokenService jwtTokenService,
    IOptions<IdentityConfig> identityConfigOptions)
{
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly RoleManager<IdentityRole> roleManager = roleManager;

    private readonly JwtTokenService jwtTokenService = jwtTokenService;
    private readonly IdentityConfig identityConfig = identityConfigOptions.Value;

    /// <summary>
    /// Checks if a user with the specified username exists.
    /// </summary>
    public async Task<bool> UserExistsAsync(string username)
        => await userManager.Users.AnyAsync(u => u.UserName == username);

    /// <summary>
    /// Authenticates the user by validating their credentials
    /// and generating a JWT token if successful.
    /// </summary>
    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.Username);

        if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
        {
            return new AuthResult
            {
                Success = false,
                Error = "Invalid username or password."
            };
        }

        var userRoles = await userManager.GetRolesAsync(user);
        var token = jwtTokenService.GenerateToken(user, userRoles);

        return new AuthResult()
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            Username = user.UserName!,
            Roles = userRoles.ToList()
        };
    }


    /// <summary>
    /// Registers a new user, creates the user in the database, and assigns a default role.
    /// </summary>
    public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
    {
        var newUser = new ApplicationUser()
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName ?? string.Empty,
            LastName = registerDto.LastName ?? string.Empty,
        };

        var result = await userManager.CreateAsync(newUser, registerDto.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, identityConfig.DefaultRole);
        }

        return result;
    }
}
