using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using MediatR;

namespace AgendaManager.Application.Authorization.Commands.UpdatePermissionForRole;

internal class UpdatePermissionForRoleCommandHandler(AuthorizationService authorizationService)
    : IRequestHandler<UpdatePermissionForRoleCommand, Result>
{
    public async Task<Result> Handle(UpdatePermissionForRoleCommand request, CancellationToken cancellationToken)
    {
        var result = request.IsAssigned
            ? await AddPermissionToRole(request.RoleId, request.PermissionId, cancellationToken)
            : await RemovePermissionFromRole(request.RoleId, request.PermissionId, cancellationToken);

        return result;
    }

    private async Task<Result> AddPermissionToRole(Guid roleId, Guid permissionId, CancellationToken cancellationToken)
    {
        var result = await authorizationService.AddPermissionToRoleAsync(
            RoleId.From(roleId),
            PermissionId.From(permissionId),
            cancellationToken);

        return result;
    }

    private async Task<Result> RemovePermissionFromRole(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken)
    {
        var result = await authorizationService.RemovePermissionFromRoleAsync(
            RoleId.From(roleId),
            PermissionId.From(permissionId),
            cancellationToken);

        return result;
    }
}
