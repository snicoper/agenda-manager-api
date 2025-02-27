﻿using AgendaManager.Application.Common.Http;
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
        // Get queryable roles.
        var roles = roleRepository.GetQueryAbleRoles();

        // Filter, order and map to response.
        var responseData = await ResponseData<GetRolesPaginatedQueryResponse>.CreateAsync(
            source: roles,
            projection: r => new GetRolesPaginatedQueryResponse(r.Id.Value, r.Name, r.Description, r.IsEditable),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return Result.Success(responseData);
    }
}
