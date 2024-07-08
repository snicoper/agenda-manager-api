using AgendaManager.Application.Users.Commands.CreateUser;
using AgendaManager.Application.Users.Commands.UpdateUser;
using AgendaManager.Application.Users.Queries.GetUsers;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Extensions;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Users;

[Route("api/v{version:apiVersion}/users")]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<Result<List<GetUsersResponse>>>> GetUsers(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetUsersQuery(string.Empty), cancellationToken);

        return result.MapToResponse(this);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Result<CreateUserResponse>>> CreateUser(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateUserCommand("test2@example.com", "sdafsdfsdfsfsdfsdfsdfsdf"),
            cancellationToken);

        return result.MapToResponse(this);
    }

    [AllowAnonymous]
    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<Result<UpdateUserResponse>>> UpdateUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new UpdateUserCommand(userId, "newtest@example.com"), cancellationToken);

        return result.MapToResponse(this);
    }
}
