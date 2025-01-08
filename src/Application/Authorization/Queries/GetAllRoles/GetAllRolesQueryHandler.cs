using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Queries.GetAllRoles;

internal sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetAllRolesQuery, IEnumerable<GetAllRolesQueryResponse>>
{
    public async Task<Result<IEnumerable<GetAllRolesQueryResponse>>> Handle(
        GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        // Get all roles with permissions.
        var roles = await roleRepository.GetAllWithPermissionsAsync(cancellationToken);

        // Map to response.
        var result = roles.Select(
            r => new GetAllRolesQueryResponse(
                Id: r.Id.Value,
                Name: r.Name,
                Description: r.Description,
                Permissions: r.Permissions.Select(
                    p => new GetAllRolesQueryResponse.Permission(
                        p.Id.Value,
                        p.Name)).ToList()));

        return Result.Success(result);
    }
}
