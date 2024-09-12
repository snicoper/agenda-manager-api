using AgendaManager.Application.Users.Commands.CreateUser;
using AgendaManager.Application.Users.Commands.UpdateUser;
using AgendaManager.Application.Users.Queries.GetUsers;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Users;

[Route("api/v{version:apiVersion}/users")]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<List<GetUsersQueryResponse>>>> GetUsers(CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery("alice@example.com");
        var result = await Sender.Send(query, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<Result<CreateUserCommandResponse>>> CreateUser(CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand("test2@example.com", "sdafsdfsdfsfsdfsdfsdfsdf");
        var result = await Sender.Send(command, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<Result<UpdateUserCommandResponse>>> UpdateUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(userId, "test2@example.com");
        var result = await Sender.Send(command, cancellationToken);

        return result.ToActionResult();
    }
}
