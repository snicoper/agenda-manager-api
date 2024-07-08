using AgendaManager.Application.Authentication.Commands.Register;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Contracts.Authentication;
using AgendaManager.WebApi.Extensions;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers;

[AllowAnonymous]
[Route("api/v{version:apiVersion}/authentication")]
public class AuthenticationController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<Result<RegisterResponse>>> Register(RegisterRequest request)
    {
        var result = await Sender.Send(
            new RegisterCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.ConfirmPassword));

        if (!result.Succeeded)
        {
            return result
                .MapTo<RegisterResponse>()
                .ToHttpResponse(this);
        }

        return Result
            .Success(new RegisterResponse(result.Value!.UserId))
            .ToHttpResponse(this);
    }

    [HttpPost("login")]
    public ActionResult<Result<LoginResponse>> Login(LoginRequest request)
    {
        throw new NotImplementedException();
    }
}
