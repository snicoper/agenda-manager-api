using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersNotInRoleIdPaginated;

internal class GetUsersNotInRoleIdPaginatedCommandHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersNotInRoleIdPaginatedCommand, ResponseData<GetUsersNotInRoleIdPaginatedCommandResponse>>
{
    public async Task<Result<ResponseData<GetUsersNotInRoleIdPaginatedCommandResponse>>> Handle(
        GetUsersNotInRoleIdPaginatedCommand request,
        CancellationToken cancellationToken)
    {
        var users = userRepository.GetQueryableUsersNotInRoleId(RoleId.From(request.RoleId));

        users = UserFilter.ApplyFilters(users, request.RequestData);

        var responseData = await ResponseData<GetUsersNotInRoleIdPaginatedCommandResponse>.CreateAsync(
            source: users,
            projection: u => new GetUsersNotInRoleIdPaginatedCommandResponse(u.Id.Value, u.Email.Value),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return Result.Success(responseData);
    }
}
