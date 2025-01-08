using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetRolesByUserId;

internal class GetRolesByUserIdQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    : IQueryHandler<GetRolesByUserIdQuery, List<GetRolesByUserIdQueryResponse>>
{
    public async Task<Result<List<GetRolesByUserIdQueryResponse>>> Handle(
        GetRolesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get user by id and check if it exists.
        var user = await userRepository.GetByIdWithRolesAsync(UserId.From(request.UserId), cancellationToken);
        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        // Get roles ids from user.
        var rolesIds = user.UserRoles.Select(x => x.RoleId).ToList();

        // Get roles by ids.
        var roles = await roleRepository.GetRolesByIdsAsync(rolesIds, cancellationToken);

        // Map roles to response.
        var response = roles.Select(role => new GetRolesByUserIdQueryResponse(role.Id.Value, role.Name)).ToList();

        return Result.Success(response);
    }
}
