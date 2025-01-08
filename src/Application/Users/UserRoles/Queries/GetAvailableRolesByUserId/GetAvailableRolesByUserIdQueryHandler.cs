using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetAvailableRolesByUserId;

internal class GetAvailableRolesByUserIdQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    : IQueryHandler<GetAvailableRolesByUserIdQuery, List<GetAvailableRolesByUserIdQueryResponse>>
{
    public async Task<Result<List<GetAvailableRolesByUserIdQueryResponse>>> Handle(
        GetAvailableRolesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get user by id and check if it exists.
        var user = await userRepository.GetByIdWithRolesAsync(UserId.From(request.UserId), cancellationToken);
        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        // Get roles ids from user.
        var rolesIds = user.UserRoles
            .Select(x => x.RoleId)
            .ToList();

        // Get all roles.
        var roles = await roleRepository.GetAllRolesAsync(cancellationToken);

        // Map roles to response.
        var response = roles
            .Select(
                role => new GetAvailableRolesByUserIdQueryResponse(
                    RoleId: role.Id.Value,
                    Name: role.Name,
                    IsAssigned: rolesIds.Any(r => r == role.Id)))
            .ToList();

        return Result.Success(response);
    }
}
