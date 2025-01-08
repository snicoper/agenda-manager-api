using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Commands.DeleteRole;

internal class DeleteRoleCommandHandler(RoleManager roleManager, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        // Delete role.
        var result = await roleManager.DeleteRoleAsync(RoleId.From(request.RoleId), cancellationToken);
        if (result.IsSuccess)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.NoContent();
    }
}
