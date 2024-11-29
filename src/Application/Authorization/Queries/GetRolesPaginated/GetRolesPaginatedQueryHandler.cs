using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Queries.GetRolesPaginated;

internal class GetRolesPaginatedQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetRolesPaginatedQuery, ResponseData<GetRolesPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetRolesPaginatedQueryResponse>>> Handle(
        GetRolesPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        var roles = roleRepository.GetQueryAbleRoles();

        var responseData = await ResponseData<GetRolesPaginatedQueryResponse>.CreateAsync(
            roles,
            r => new GetRolesPaginatedQueryResponse(r.Id.Value, r.Name, r.Description),
            request.RequestData,
            cancellationToken);

        return responseData;
    }
}
