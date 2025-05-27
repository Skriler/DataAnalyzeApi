namespace DataAnalyzeApi.Models.Config.Identity;

public class IdentityConfig
{
    public List<string> Roles { get; set; } = new();

    public string AdminRole { get; set; } = string.Empty;

    public string DefaultRole { get; set; } = string.Empty;

    public AdminUserConfig AdminUser { get; set; } = new();
}
