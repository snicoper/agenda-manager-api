using AgendaManager.Application.Authentication.Commands.Login;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Controllers.Authentication.Contracts;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Authentication;

[AllowAnonymous]
[Route("api/v{version:apiVersion}/authentication")]
public class AuthenticationController : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<Result<TokenResult>>> Login(LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
