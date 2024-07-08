using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Commands.UpdateUser;

internal class UpdateUserHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
{
    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await usersRepository.GetByIdAsync(UserId.From(request.Id), cancellationToken);

        if (existingUser is null)
        {
            return Error
                .NotFound(nameof(User), nameof(UserId))
                .ToResult<UpdateUserResponse>();
        }

        var updatedUser = existingUser.UpdateEmail(EmailAddress.From(request.Email));
        usersRepository.Update(existingUser, updatedUser);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateUserResponse(request.Email));
    }
}
