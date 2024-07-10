using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Commands.UpdateUser;

internal class UpdateUserCommandHandler(IUserRepository usersRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand, UpdateUserCommandResponse>
{
    public async Task<Result<UpdateUserCommandResponse>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(UserId.From(request.Id), cancellationToken);

        if (user is null)
        {
            return Error
                .NotFound("User not found")
                .ToResult<UpdateUserCommandResponse>();
        }

        user.UpdateEmail(EmailAddress.From(request.Email));
        usersRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateUserCommandResponse(request.Email));
    }
}
