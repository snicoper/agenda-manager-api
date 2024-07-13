using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgendaManager.Infrastructure.Users;

public class JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, IAuthorizationManager authorizationManager)
    : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<TokenResult> GenerateAccessTokenAsync(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var id = user.Id.Value.ToString();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, id),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new(JwtRegisteredClaimNames.FamilyName, $"{user.FirstName} {user.LastName}"),
            new(CustomClaimType.Id, id)
        };

        await AddRolesClaim(user, claims);
        await AddPermissionsClaim(user, claims);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.AccessTokenLifeTimeMinutes),
            signingCredentials: credentials);

        var jwtSecurityToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();
        var expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.RefreshTokenLifeTimeDays);

        var tokenResponse = new TokenResult(jwtSecurityToken, refreshToken, expires);

        return tokenResponse;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        var tokenRefresh = Convert.ToBase64String(randomNumber);

        return tokenRefresh;
    }

    private async Task AddRolesClaim(User user, List<Claim> claims)
    {
        var roles = await authorizationManager.GetRolesByUserIdAsync(user.Id);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
    }

    private async Task AddPermissionsClaim(User user, List<Claim> claims)
    {
        var permissions = await authorizationManager.GetPermissionsByUserIdAsync(user.Id);

        claims.AddRange(permissions.Select(permission => new Claim(CustomClaimType.Permissions, permission.Name)));
    }
}
