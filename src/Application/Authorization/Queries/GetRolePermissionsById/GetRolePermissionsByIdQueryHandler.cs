using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Queries.GetRolePermissionsById;

internal class GetRolePermissionsByIdQueryHandler(
    IRoleRepository repository,
    IPermissionRepository permissionRepository)
    : IQueryHandler<GetRolePermissionsByIdQuery, GetRolePermissionsByIdQueryResponse>
{
    public async Task<Result<GetRolePermissionsByIdQueryResponse>> Handle(
        GetRolePermissionsByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get role and check if exists.
        var role = await repository.GetByIdWithPermissionsAsync(RoleId.From(request.RoleId), cancellationToken);
        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        // Get all permissions from role.
        var permissionIdsFromRole = role.Permissions
            .Select(x => x)
            .ToHashSet();

        var permissions = await permissionRepository.GetAllAsync(cancellationToken);

        // Agrupar los permisos por módulo.
        var modulePermissions = permissions
            .GroupBy(p => p.Name.Split(':')[0])
            .Select(
                group => new GetRolePermissionsByIdQueryResponse.ModulePermission(
                    group.Key,
                    group.Select(
                        p => new GetRolePermissionsByIdQueryResponse.PermissionDetail(
                            PermissionId: p.Id.Value,
                            Action: p.Name.Split(':')[1],
                            IsAssigned: permissionIdsFromRole.Any(rp => rp.Id == p.Id))).ToList()))
            .ToList();

        // Map to response.
        var response = new GetRolePermissionsByIdQueryResponse(
            RoleId: role.Id.Value,
            RoleName: role.Name,
            RoleDescription: role.Description,
            RoleIsEditable: role.IsEditable,
            Permissions: modulePermissions);

        return response;
    }
}
