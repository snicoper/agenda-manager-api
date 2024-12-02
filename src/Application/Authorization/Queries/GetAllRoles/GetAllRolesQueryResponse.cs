namespace AgendaManager.Application.Authorization.Queries.GetAllRoles;

public record GetAllRolesQueryResponse(
    Guid Id,
    string Name,
    string Description,
    IReadOnlyList<GetAllRolesQueryResponse.Permission> Permissions)
{
    public record Permission(Guid PermissionId, string Name);
}
