namespace AgendaManager.Application.Authorization.Queries.GetRolePermissionsById;

public record GetRolePermissionsByIdQueryResponse(
    Guid RoleId,
    string RoleName,
    string RoleDescription,
    bool RoleIsEditable,
    List<GetRolePermissionsByIdQueryResponse.ModulePermission> Permissions)
{
    public record ModulePermission(string ModuleName, List<PermissionDetail> Permissions);

    public record PermissionDetail(Guid PermissionId, string Action, bool IsAssigned);
}
