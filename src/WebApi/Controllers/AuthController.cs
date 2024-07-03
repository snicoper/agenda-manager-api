using AgendaManager.Application.Auth.Commands.Login;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers;

[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<Result<LoginResponse>>> Login(LoginCommand command)
    {
        var result = await Sender.Send(command);

        return ToHttpResponse(result);
    }
}
