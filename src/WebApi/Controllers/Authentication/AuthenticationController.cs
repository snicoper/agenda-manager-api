using AgendaManager.Application.Authentication.Commands.Register;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Extensions;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Authentication;

[AllowAnonymous]
[Route("api/v{version:apiVersion}/authentication")]
public class AuthenticationController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<Result<RegisterCommandResponse>>> Register(RegisterRequest request)
    {
        var result = await Sender.Send(
            new RegisterCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.ConfirmPassword));

        return result.MapToResponse(this);
    }

    [HttpPost("login")]
    public ActionResult<Result> Login(LoginRequest request)
    {
        return Result.Failure().MapToResponse(this);
    }
}
