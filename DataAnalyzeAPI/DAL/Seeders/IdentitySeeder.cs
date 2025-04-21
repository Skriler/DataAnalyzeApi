using DataAnalyzeAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace DataAnalyzeAPI.DAL.Seeders;

public class IdentitySeeder
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration configuration;

    public IdentitySeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.configuration = configuration;
    }

    public async Task SeedRolesAsync()
    {
        var roleNames = new string[]{ "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
