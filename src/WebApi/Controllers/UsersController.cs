using AgendaManager.Application.Users.Commands.CreateUser;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Extensions;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers;

[Route("api/v{version:apiVersion}/users")]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<Result<CreateUserResponse>>> CreateUser(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new CreateUserCommand("test2@example.com", "sdafsdfsdfsfsdfsdfsdfsdf"), cancellationToken);

        return result.ToHttpResponse(this);
    }
}
