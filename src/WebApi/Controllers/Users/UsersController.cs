using AgendaManager.Application.Users.Commands.CreateUser;
using AgendaManager.Application.Users.Commands.UpdateUser;
using AgendaManager.Application.Users.Queries.GetUsers;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Users;

[Route("api/v{version:apiVersion}/users")]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<List<GetUsersQueryResponse>>>> GetUsers(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetUsersQuery(string.Empty), cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<Result<CreateUserCommandResponse>>> CreateUser(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateUserCommand("test2@example.com", "sdafsdfsdfsfsdfsdfsdfsdf"),
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<Result<UpdateUserCommandResponse>>> UpdateUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new UpdateUserCommand(userId, "newtest@example.com"), cancellationToken);

        return result.ToActionResult();
    }
}
