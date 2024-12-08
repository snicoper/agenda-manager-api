using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersByRoleId;

internal class GetUsersByRoleIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersByRoleIdQuery, ResponseData<GetUsersByRoleIdQueryResponse>>
{
    public async Task<Result<ResponseData<GetUsersByRoleIdQueryResponse>>> Handle(
        GetUsersByRoleIdQuery request,
        CancellationToken cancellationToken)
    {
        var users = userRepository.GetQueryableUsersByRoleId(RoleId.From(request.RoleId));
        users = UserFilter.ApplyFilters(users, request.RequestData);

        var responseData = await ResponseData<GetUsersByRoleIdQueryResponse>.CreateAsync(
            source: users,
            projection: u => new GetUsersByRoleIdQueryResponse(u.Id.Value, u.Email.Value),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return Result.Success(responseData);
    }
}
