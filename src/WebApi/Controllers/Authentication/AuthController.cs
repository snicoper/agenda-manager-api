using AgendaManager.Application.Authentication.Commands.Login;
using AgendaManager.Application.Authentication.Commands.RefreshToken;
using AgendaManager.Application.Authentication.Models;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Controllers.Authentication.Contracts;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Authentication;

[AllowAnonymous]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ApiControllerBase
{
    /// <summary>
    /// Authenticates the user.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("login")]
    public async Task<ActionResult<Result<TokenResult>>> Login(LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Refreshes the user's token.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<Result<TokenResult>>> RefreshToken(RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
