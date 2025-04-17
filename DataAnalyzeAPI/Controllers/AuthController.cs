using DataAnalyzeAPI.Models.DTOs.Auth;
using DataAnalyzeAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    // TODO
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModelDto dto)
    {
        return Unauthorized();
    }

    // TODO
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
    {
        return Unauthorized();
    }
}
