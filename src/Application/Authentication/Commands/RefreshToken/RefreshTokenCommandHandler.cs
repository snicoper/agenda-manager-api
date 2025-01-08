using AgendaManager.Application.Authentication.Interfaces;
using AgendaManager.Application.Authentication.Models;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Authentication.Commands.RefreshToken;

internal class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RefreshTokenCommand, TokenResult>
{
    public async Task<Result<TokenResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Get the user and check if it is active and has a valid refresh token.
        var user = await userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user is null || !user.IsActive || user.RefreshToken is null || user.RefreshToken.IsExpired)
        {
            throw new UnauthorizedAccessException();
        }

        // Generate a new access token and refresh token.
        var tokenResult = await jwtTokenGenerator.GenerateAccessTokenAsync(user.Id, cancellationToken);
        var refreshToken = Token.From(
            tokenResult.RefreshToken,
            tokenResult.Expires);

        // Update the user refresh token.
        user.UpdateRefreshToken(refreshToken);
        userRepository.Update(user);

        // Save the changes to the database.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create(tokenResult);
    }
}
