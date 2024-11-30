namespace AgendaManager.Application.Authorization.Queries.GetRoleWithPermissionAvailabilityById;

public record GetRoleWithPermissionAvailabilityByIdQueryResponse(
    Guid RoleId,
    string RoleName,
    string RoleDescription,
    List<GetRoleWithPermissionAvailabilityByIdQueryResponse.ModulePermission> Permissions)
{
    public record ModulePermission(string ModuleName, List<Permission> Permissions);

    public record Permission(Guid PermissionId, string Action, bool IsAssigned);
}
