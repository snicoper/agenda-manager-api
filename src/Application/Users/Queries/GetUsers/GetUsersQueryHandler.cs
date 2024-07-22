using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Queries.GetUsers;

internal class GetUsersQueryHandler(IUserRepository usersRepository, UserAuthorizationManager userAuthorizationManager)
    : IQueryHandler<GetUsersQuery, List<GetUsersQueryResponse>>
{
    public async Task<Result<List<GetUsersQueryResponse>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByEmailAsync(EmailAddress.From(request.Email), cancellationToken);

        if (user is null)
        {
            return Error.NotFound();
        }

        var userWithRole = await usersRepository.GetByIdWithRolesAsync(user.Id, cancellationToken);

        if (userWithRole is null)
        {
            return Error.NotFound();
        }

        var userRoles = userWithRole.Roles.ToList();

        var addRoleResult =
            await userAuthorizationManager.AddRoleToUserAsync(user.Id, userRoles[0].Id, cancellationToken);

        if (addRoleResult.IsFailure)
        {
            return addRoleResult.MapToValue<List<GetUsersQueryResponse>>();
        }

        var users = usersRepository
            .GetQueryable()
            .Select(u => new GetUsersQueryResponse(u.Id.Value, u.Email.Value, u.FirstName, u.LastName))
            .ToList();

        return Result.Success(users);
    }
}
