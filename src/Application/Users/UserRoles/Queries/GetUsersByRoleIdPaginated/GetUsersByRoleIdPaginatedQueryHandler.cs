using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersByRoleIdPaginated;

internal class GetUsersByRoleIdPaginatedQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersByRoleIdPaginatedQuery, ResponseData<GetUsersByRoleIdPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetUsersByRoleIdPaginatedQueryResponse>>> Handle(
        GetUsersByRoleIdPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        var users = userRepository.GetQueryableUsersByRoleId(RoleId.From(request.RoleId));
        users = UserFilter.ApplyFilters(users, request.RequestData);

        var responseData = await ResponseData<GetUsersByRoleIdPaginatedQueryResponse>.CreateAsync(
            source: users,
            projection: u => new GetUsersByRoleIdPaginatedQueryResponse(u.Id.Value, u.Email.Value),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return Result.Success(responseData);
    }
}
