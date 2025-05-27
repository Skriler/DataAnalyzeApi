namespace DataAnalyzeApi.Models.Config;

public class PostgresConfig
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
