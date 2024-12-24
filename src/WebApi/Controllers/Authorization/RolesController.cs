using AgendaManager.Application.Authorization.Commands.CreateRole;
using AgendaManager.Application.Authorization.Commands.DeleteRole;
using AgendaManager.Application.Authorization.Commands.UpdatePermissionForRole;
using AgendaManager.Application.Authorization.Commands.UpdateRole;
using AgendaManager.Application.Authorization.Queries.GetAllRoles;
using AgendaManager.Application.Authorization.Queries.GetRoleById;
using AgendaManager.Application.Authorization.Queries.GetRolePermissionsById;
using AgendaManager.Application.Authorization.Queries.GetRolesPaginated;
using AgendaManager.Application.Common.Http;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Controllers.Authorization.Contracts;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Authorization;

[Route("api/v{version:apiVersion}/roles")]
public class RolesController : ApiControllerBase
{
    /// <summary>
    /// Obtener una lista de roles paginada.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetRolesPaginatedQueryResponse>>>> GetRolesPaginated(
        [FromQuery] RequestData request)
    {
        var query = new GetRolesPaginatedQuery(request);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Obtener una lista de todos los roles.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<Result<IEnumerable<GetAllRolesQueryResponse>>>> GetAllRoles()
    {
        var query = new GetAllRolesQuery();
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Obtener un rol por su ID.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{roleId:guid}")]
    public async Task<ActionResult<Result<GetRoleByIdQueryResponse>>> GetRoleById(Guid roleId)
    {
        var query = new GetRoleByIdQuery(roleId);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Obtener los permisos de un rol por su ID.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpGet("{roleId:guid}/permissions")]
    public async Task<ActionResult<Result<GetRolePermissionsByIdQueryResponse>>> GetRolePermissionsById(Guid roleId)
    {
        var query = new GetRolePermissionsByIdQuery(roleId);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Crear un nuevo rol.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<Result<CreateRoleCommandResponse>>> CreateRole(CreateRoleRequest request)
    {
        var command = new CreateRoleCommand(request.Name, request.Description);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Actualizar un rol.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPut("{roleId:guid}")]
    public async Task<ActionResult<Result>> UpdateRole(UpdateRoleRequest request, Guid roleId)
    {
        var command = new UpdateRoleCommand(roleId, request.Name, request.Description);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Actualizar un permiso de un rol.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPut("{roleId:guid}/permissions/{permissionId:guid}")]
    public async Task<ActionResult<Result>> UpdatePermissionForRole(
        UpdatePermissionForRoleRequest request,
        Guid roleId,
        Guid permissionId)
    {
        var command = new UpdatePermissionForRoleCommand(roleId, permissionId, request.IsAssigned);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Eliminar un rol.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{roleId:guid}")]
    public async Task<ActionResult<Result>> DeleteRole(Guid roleId)
    {
        var command = new DeleteRoleCommand(roleId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
