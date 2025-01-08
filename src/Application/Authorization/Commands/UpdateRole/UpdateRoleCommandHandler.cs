using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Commands.UpdateRole;

internal class UpdateRoleCommandHandler(RoleManager roleManager, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        // Get role and check if exists.
        var role = await roleRepository.GetByIdAsync(RoleId.From(request.RoleId), cancellationToken);
        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        // Update role.
        var result = await roleManager.UpdateRoleAsync(
            role,
            request.Name,
            request.Description,
            cancellationToken);

        if (result.IsSuccess)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.NoContent();
    }
}
