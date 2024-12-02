using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Queries.GetRoleById;

internal class GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResponse>
{
    public async Task<Result<GetRoleByIdQueryResponse>> Handle(
        GetRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(RoleId.From(request.Id), cancellationToken);

        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        var response = new GetRoleByIdQueryResponse(
            role.Id.Value,
            role.Name,
            role.Description,
            role.IsEditable);

        return response;
    }
}
