using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Queries.GetRoleWithPermissionAvailabilityById;

internal class GetRoleWithPermissionAvailabilityByIdQueryHandler(
    IRoleRepository repository,
    IPermissionRepository permissionRepository)
    : IQueryHandler<GetRoleWithPermissionAvailabilityByIdQuery, GetRoleWithPermissionAvailabilityByIdQueryResponse>
{
    public async Task<Result<GetRoleWithPermissionAvailabilityByIdQueryResponse>> Handle(
        GetRoleWithPermissionAvailabilityByIdQuery request,
        CancellationToken cancellationToken)
    {
        var role = await repository.GetByIdWithPermissionsAsync(RoleId.From(request.RoleId), cancellationToken);
        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        var permissionIdsFromRole = role.Permissions
            .Select(x => x)
            .ToHashSet();

        var permissions = await permissionRepository.GetAllAsync(cancellationToken);

        // Agrupar los permisos por módulo.
        var modulePermissions = permissions
            .GroupBy(p => p.Name.Split(':')[0])
            .Select(
                group => new GetRoleWithPermissionAvailabilityByIdQueryResponse.ModulePermission(
                    group.Key,
                    group.Select(
                        p => new GetRoleWithPermissionAvailabilityByIdQueryResponse.Permission(
                            PermissionId: p.Id.Value,
                            Action: p.Name.Split(':')[1],
                            IsAssigned: permissionIdsFromRole.Any(rp => rp.Id == p.Id))).ToList()))
            .ToList();

        var response = new GetRoleWithPermissionAvailabilityByIdQueryResponse(
            RoleId: role.Id.Value,
            RoleName: role.Name,
            RoleDescription: role.Description,
            RoleIsEditable: role.IsEditable,
            Permissions: modulePermissions);

        return response;
    }
}
