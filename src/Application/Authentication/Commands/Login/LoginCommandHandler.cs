using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Authentication.Commands.Login;

internal class LoginCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    UserAuthenticationService userAuthenticationService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<LoginCommand, TokenResult>
{
    public async Task<Result<TokenResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(EmailAddress.From(request.Email), cancellationToken);

        if (user is null)
        {
            return UserErrors.InvalidCredentials;
        }

        var authResult = userAuthenticationService.AuthenticateUser(user, request.Password);

        if (authResult.IsFailure)
        {
            return authResult.MapToValue<TokenResult>();
        }

        var tokenResult = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = RefreshToken.Create(tokenResult.RefreshToken, tokenResult.Expires);

        user.UpdateRefreshToken(refreshToken);
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(tokenResult);
    }
}
