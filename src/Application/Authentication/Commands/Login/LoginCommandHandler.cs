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
    UserPasswordService userPasswordService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<LoginCommand, TokenResult>
{
    public async Task<Result<TokenResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(EmailAddress.From(request.Email), cancellationToken);

        if (user is null)
        {
            return UserErrors.InvalidCredentials.ToResult<TokenResult>();
        }

        if (!userPasswordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            return UserErrors.InvalidCredentials.ToResult<TokenResult>();
        }

        if (!user.Active)
        {
            return UserErrors.UserIsNotActive.ToResult<TokenResult>();
        }

        var tokenResponse = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = RefreshToken.Create(tokenResponse.RefreshToken, tokenResponse.Expires);

        user.UpdateRefreshToken(refreshToken);
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(tokenResponse);
    }
}
