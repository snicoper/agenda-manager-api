using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Settings;
using AgendaManager.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgendaManager.Infrastructure.Users;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, UserManager<User> userManager)
    : IJwtTokenGenerator
{
    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var claims = new List<Claim> { new(ClaimTypes.Sid, user.Id.Value.ToString()), new(ClaimTypes.Name, user.UserName) };

        var roles = await userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(
            jwtSettings.Value.Issuer,
            jwtSettings.Value.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtSettings.Value.AccessTokenLifeTimeMinutes),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        var tokenRefresh = Convert.ToBase64String(randomNumber);

        return tokenRefresh;
    }
}
