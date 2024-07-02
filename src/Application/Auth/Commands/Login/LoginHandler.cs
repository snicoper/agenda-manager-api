using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Auth.Commands.Login;

internal sealed class LoginHandler(IAuthService authService)
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request.Email, request.Password);

        if (!result.Succeeded || !result.HasValue)
        {
            return result.MapTo<LoginResponse>();
        }

        var resultResponse = new LoginResponse(result.Value!.AccessToken, result.Value.RefreshToken);

        return resultResponse;
    }
}
