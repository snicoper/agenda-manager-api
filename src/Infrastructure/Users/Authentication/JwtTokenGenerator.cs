using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendaManager.Application.Authentication.Interfaces;
using AgendaManager.Application.Authentication.Models;
using AgendaManager.Application.Common.Exceptions;
using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgendaManager.Infrastructure.Users.Authentication;

public class JwtTokenGenerator(
    IOptions<JwtSettings> jwtOptions,
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<TokenResult> GenerateAccessTokenAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(User), nameof(UserId));
        }

        var roleIds = user.UserRoles.Select(x => x.RoleId).ToList();
        var roles = await roleRepository.GetByIdsWithPermissionsAsync(roleIds, cancellationToken);

        var claims = CreateClaimsForUser(user, roles.ToList());

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.AccessTokenLifeTimeMinutes),
            signingCredentials: credentials);

        var jwtSecurityToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshTokenLifetime = TimeSpan.FromDays(_jwtSettings.RefreshTokenLifeTimeDays);
        var refreshToken = Token.Generate(refreshTokenLifetime);

        var tokenResponse = new TokenResult(jwtSecurityToken, refreshToken.Value, refreshToken.Expires);

        return tokenResponse;
    }

    private static List<Claim> CreateClaimsForUser(User user, List<Role> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(CustomClaimType.Id, user.Id.Value.ToString())
        };

        claims = SetClaimRolesAndPermissions(roles, claims);

        return claims;
    }

    private static List<Claim> SetClaimRolesAndPermissions(List<Role> roles, List<Claim> claims)
    {
        var permissions = roles.SelectMany(u => u.Permissions).ToList();
        var permissionsNames = permissions
            .Select(permission => permission.Name)
            .Distinct()
            .ToList();

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        claims.AddRange(
            permissionsNames.Select(permissionName => new Claim(CustomClaimType.Permissions, permissionName)));

        return claims;
    }
}
