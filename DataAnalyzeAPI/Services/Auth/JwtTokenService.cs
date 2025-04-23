using DataAnalyzeAPI.Models.Config;
using DataAnalyzeAPI.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAnalyzeAPI.Services.Auth;

public class JwtTokenService
{
    private readonly JwtConfig jwtConfig;

    public JwtTokenService(IOptions<JwtConfig> jwtOptions)
    {
        jwtConfig = jwtOptions.Value;
    }

    /// <summary>
    /// Generates a JWT token containing user information and roles.
    /// </summary>
    public JwtSecurityToken GenerateToken(ApplicationUser user, IEnumerable<string> roles)
    {
        var authClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));

        return new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: authClaims,
            expires: DateTime.Now.AddMinutes(jwtConfig.ExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
    }
}
