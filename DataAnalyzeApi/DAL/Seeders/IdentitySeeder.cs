using DataAnalyzeApi.Models.Config.Identity;
using DataAnalyzeApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DataAnalyzeApi.DAL.Seeders;

public class IdentitySeeder
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    private readonly IdentityConfig identityConfig;
    private readonly ILogger<IdentitySeeder> logger;

    public IdentitySeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityConfig> identityConfigOptions,
        ILogger<IdentitySeeder> logger)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;

        identityConfig = identityConfigOptions.Value;
        this.logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
            logger.LogInformation("Identity seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding identity data.");
            throw;
        }
    }

    /// <summary>
    /// Seeds roles defined in configuration if they do not exist.
    /// </summary>
    private async Task SeedRolesAsync()
    {
        var roles = identityConfig.Roles;

        if (roles == null || roles.Count == 0)
        {
            logger.LogWarning("No roles configured for seeding.");
            return;
        }

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    /// <summary>
    /// Seeds the admin user defined in configuration if not already exists.
    /// </summary>
    private async Task SeedAdminUserAsync()
    {
        var adminConfig = identityConfig.AdminUser;

        var existingAdmin = await userManager.FindByNameAsync(adminConfig.Username);

        if (existingAdmin != null)
        {
            logger.LogWarning("Admin already exists.");
            return;
        }

        var user = new ApplicationUser
        {
            UserName = adminConfig.Username,
            Email = adminConfig.Email,
            EmailConfirmed = true,
            FirstName = adminConfig.FirstName,
            LastName = adminConfig.LastName,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await userManager.CreateAsync(user, adminConfig.Password);

        if (!result.Succeeded)
        {
            logger.LogWarning("Failed to create user: {Username}, Errors: {Errors}",
                adminConfig.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
            return;
        }

        await userManager.AddToRoleAsync(user, identityConfig.AdminRole);
    }
}
