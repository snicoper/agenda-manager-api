using AgendaManager.Application.Authorization.Commands.CreateRole;
using AgendaManager.Application.Authorization.Commands.DeleteRole;
using AgendaManager.Application.Authorization.Commands.UpdatePermissionForRole;
using AgendaManager.Application.Authorization.Queries.GetAllRoles;
using AgendaManager.Application.Authorization.Queries.GetRolesPaginated;
using AgendaManager.Application.Authorization.Queries.GetRoleWithPermissionAvailabilityById;
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
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetRolesPaginatedQueryResponse>>>> GetRolesPaginated(
        [FromQuery] RequestData request)
    {
        var result = await Sender.Send(new GetRolesPaginatedQuery(request));

        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<ActionResult<Result<IEnumerable<GetAllRolesQueryResponse>>>> GetAllRoles()
    {
        var result = await Sender.Send(new GetAllRolesQuery());

        return result.ToActionResult();
    }

    [HttpGet("{roleId:guid}/permission-availability")]
    public async Task<ActionResult<Result<GetRoleWithPermissionAvailabilityByIdQueryResponse>>>
        GetRoleWithPermissionAvailabilityById(Guid roleId)
    {
        var result = await Sender.Send(new GetRoleWithPermissionAvailabilityByIdQuery(roleId));

        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<Result<CreateRoleCommandResponse>>> CreateRole(CreateRoleRequest request)
    {
        var command = new CreateRoleCommand(request.Name, request.Description);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

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

    [HttpDelete("{roleId:guid}")]
    public async Task<ActionResult<Result>> DeleteRole(Guid roleId)
    {
        var command = new DeleteRoleCommand(roleId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
