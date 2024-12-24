using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Users.UserRoles.Commands.AssignUserToRole;
using AgendaManager.Application.Users.UserRoles.Commands.UnAssignedUserFromRole;
using AgendaManager.Application.Users.UserRoles.Queries.GetAvailableRolesByUserId;
using AgendaManager.Application.Users.UserRoles.Queries.GetRolesByUserId;
using AgendaManager.Application.Users.UserRoles.Queries.GetUsersByRoleIdPaginated;
using AgendaManager.Application.Users.UserRoles.Queries.GetUsersNotInRoleIdPaginated;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Users.UserRoles;

[Route("api/v{version:apiVersion}/user-roles")]
public class UserRolesController : ApiControllerBase
{
    /// <summary>
    /// Obtener lista de usuarios en un rol por su id.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{roleId:guid}/users/paginated")]
    public async Task<ActionResult<Result<ResponseData<GetUsersByRoleIdPaginatedQueryResponse>>>>
        GetUsersByRoleIdPaginated(
            Guid roleId,
            [FromQuery] RequestData requestData)
    {
        var query = new GetUsersByRoleIdPaginatedQuery(roleId, requestData);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Obtener lista de usuarios no asignados a un rol por su id.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{roleId:guid}/exclude-users/paginated")]
    public async Task<ActionResult<Result<ResponseData<GetUsersNotInRoleIdPaginatedCommandResponse>>>>
        GetUsersNotInRoleIdPaginated(Guid roleId, [FromQuery] RequestData requestData)
    {
        var command = new GetUsersNotInRoleIdPaginatedCommand(roleId, requestData);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Obtener lista de roles de un usuario por su id.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{userId:guid}/roles")]
    public async Task<ActionResult<Result<List<GetRolesByUserIdQueryResponse>>>> GetRolesByUserId(Guid userId)
    {
        var query = new GetRolesByUserIdQuery(userId);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Obtener lista de roles disponibles para asignar a un usuario por su id,
    /// indicando cuáles ya están asignados.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{userId:guid}/available-roles")]
    public async Task<ActionResult<Result<List<GetAvailableRolesByUserIdQueryResponse>>>> GetAvailableRolesByUserId(
        Guid userId)
    {
        var query = new GetAvailableRolesByUserIdQuery(userId);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Asignar un usuario a un rol.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("{roleId:guid}/assign-user/{userId:guid}")]
    public async Task<ActionResult<Result>> AssignUserToRole(Guid roleId, Guid userId)
    {
        var command = new AssignUserToRoleCommand(roleId, userId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Des asignar un usuario de un rol.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpDelete("{roleId:guid}/unassigned-user/{userId:guid}")]
    public async Task<ActionResult<Result>> UnAssignedUserFromRole(Guid roleId, Guid userId)
    {
        var command = new UnAssignedUserFromRoleCommand(roleId, userId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
