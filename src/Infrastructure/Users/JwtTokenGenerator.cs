using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AgendaManager.Application.Common.Exceptions;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgendaManager.Infrastructure.Users;

public class JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, IUserRepository userRepository)
    : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<TokenResult> GenerateAccessTokenAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var user = await userRepository.GetByIdWithRolesAndPermissionsAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException();
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.FamilyName, $"{user.FirstName} {user.LastName}"),
            new(CustomClaimType.Id, user.Id.Value.ToString())
        };

        var permissions = user.Roles.SelectMany(u => u.Permissions).ToList();
        var permissionsNames = permissions.Select(permission => permission.Name).Distinct().ToList();

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        claims.AddRange(permissionsNames.Select(permission => new Claim(CustomClaimType.Permissions, permission)));

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.AccessTokenLifeTimeMinutes),
            signingCredentials: credentials);

        var jwtSecurityToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshTokenLifetime = TimeSpan.FromDays(_jwtOptions.RefreshTokenLifeTimeDays);
        var refreshToken = RefreshToken.Generate(refreshTokenLifetime);

        var tokenResponse = new TokenResult(jwtSecurityToken, refreshToken.Token, refreshToken.ExpiryTime);

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
}
