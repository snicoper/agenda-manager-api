using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Commands.CreateRole;

internal class CreateRoleCommandHandler(RoleManager roleManager, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoleCommand, CreateRoleCommandResponse>
{
    public async Task<Result<CreateRoleCommandResponse>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        // Create role.
        var roleResult = await roleManager.CreateRoleAsync(
            roleId: RoleId.Create(),
            name: request.Name,
            description: request.Description,
            isEditable: true,
            cancellationToken: cancellationToken);

        if (roleResult.IsFailure)
        {
            return roleResult.MapToValue<CreateRoleCommandResponse>();
        }

        // Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Return response.
        var response = new CreateRoleCommandResponse(roleResult.Value!.Id.Value);

        return Result.Create(response);
    }
}
