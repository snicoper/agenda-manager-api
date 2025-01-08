using AgendaManager.Application.Authentication.Interfaces;
using AgendaManager.Application.Authentication.Models;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;

namespace AgendaManager.Application.Authentication.Commands.Login;

internal class LoginCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    AuthenticationService authenticationService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<LoginCommand, TokenResult>
{
    public async Task<Result<TokenResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Get user and validate if exists.
        var user = await userRepository.GetByEmailAsync(EmailAddress.From(request.Email), cancellationToken);
        if (user is null)
        {
            return UserErrors.InvalidCredentials;
        }

        // Authenticate user.
        var authResult = authenticationService.AuthenticateUser(user, request.Password);
        if (authResult.IsFailure)
        {
            return authResult.MapToValue<TokenResult>();
        }

        // Generate access token and refresh token.
        var tokenResult = await jwtTokenGenerator.GenerateAccessTokenAsync(user.Id, cancellationToken);
        var refreshToken = Token.From(
            tokenResult.RefreshToken,
            tokenResult.Expires);

        // Update refresh token in user.
        user.UpdateRefreshToken(refreshToken);
        userRepository.Update(user);

        // Save changes in database.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create(tokenResult);
    }
}
