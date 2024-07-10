using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Authentication.Commands.Login;

internal class LoginCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository usersRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
    : ICommandHandler<LoginCommand, TokenResult>
{
    public async Task<Result<TokenResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByEmailAsync(EmailAddress.From(request.Email), cancellationToken);

        if (user is null)
        {
            return Error.Conflict("Invalid credentials").ToResult<TokenResult>();
        }

        if (!user.VerifyPassword(request.Password, passwordHasher))
        {
            return Error.Conflict("Invalid credentials").ToResult<TokenResult>();
        }

        if (!user.Active)
        {
            return Error.Conflict("User is not active").ToResult<TokenResult>();
        }

        var tokenResponse = await jwtTokenGenerator.GenerateAccessTokenAsync(user);
        var refreshToken = RefreshToken.Create(tokenResponse.RefreshToken, tokenResponse.Expires);

        user.UpdateRefreshToken(refreshToken);
        usersRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(tokenResponse);
    }
}
