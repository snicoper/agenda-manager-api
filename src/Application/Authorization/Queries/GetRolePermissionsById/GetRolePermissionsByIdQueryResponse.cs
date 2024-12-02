namespace AgendaManager.Application.Authorization.Queries.GetRolePermissionsById;

public record GetRolePermissionsByIdQueryResponse(
    Guid RoleId,
    string RoleName,
    string RoleDescription,
    bool RoleIsEditable,
    List<GetRolePermissionsByIdQueryResponse.ModulePermission> Permissions)
{
    public record ModulePermission(string ModuleName, List<Permission> Permissions);

    public record Permission(Guid PermissionId, string Action, bool IsAssigned);
}
