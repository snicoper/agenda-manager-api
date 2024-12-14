using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Commands.ToggleIsActive;

internal class ToggleIsActiveCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ToggleIsActiveCommand>
{
    public async Task<Result> Handle(ToggleIsActiveCommand request, CancellationToken cancellationToken)
    {
        // 1. Get user by id and check if it exists.
        var user = await userRepository.GetByIdAsync(UserId.From(request.UserId), cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        // 2. Toggle user active status.
        if (user.IsActive)
        {
            user.Deactivate();
        }
        else
        {
            user.Activate();
        }

        // 3. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
