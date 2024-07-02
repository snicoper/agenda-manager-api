using AgendaManager.Application.Common.Constants;
using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Localization;
using AgendaManager.Application.Common.Models.Settings;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Users;

public class AuthService(
    UserManager<User> userManager,
    IOptions<JwtSettings> jwtSettings,
    IJwtTokenGenerator jwtTokenGenerator,
    IDateTimeProvider dateTimeProvider,
    IStringLocalizer<UserResource> localizer)
    : IAuthService
{
    public async Task<Result<TokenResponse>> LoginAsync(string email, string password)
    {
        var user = userManager.Users.SingleOrDefault(au => au.Email == email);

        if (user is null || !await userManager.CheckPasswordAsync(user, password))
        {
            return Error.Unauthorized<TokenResponse>("Invalid username or password.");
        }

        if (!user.EmailConfirmed)
        {
            var errorMessage = localizer["El correo esta pendiente de validación desde tu bandeja de correo."];

            return Error.Validation<TokenResponse>(ValidationErrors.NonFieldErrors, errorMessage);
        }

        if (!user.Active)
        {
            var errorMessage = localizer["La cuenta no esta activa, debes hablar con un responsable de tu empresa."];

            return Error.Validation<TokenResponse>(ValidationErrors.NonFieldErrors, errorMessage);
        }

        var tokensResult = await GenerateUserTokenAsync(user);

        return tokensResult;
    }

    public async Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken)
    {
        var user = userManager.Users.SingleOrDefault(au => au.RefreshToken == refreshToken);

        if (user is null || !user.Active || user.RefreshTokenExpiryTime <= dateTimeProvider.UtcNow)
        {
            return Error.Unauthorized<TokenResponse>();
        }

        var tokensResult = await GenerateUserTokenAsync(user);

        return tokensResult;
    }

    private async Task<Result<TokenResponse>> GenerateUserTokenAsync(User user)
    {
        var jwt = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = dateTimeProvider.UtcNow.AddDays(jwtSettings.Value.RefreshTokenLifeTimeDays);

        await userManager.UpdateAsync(user);
        var userTokensResult = new TokenResponse(jwt, refreshToken);

        return Result.Success(userTokensResult);
    }
}
