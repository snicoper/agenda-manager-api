using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Commands.UpdateUser;

internal class UpdateUserHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
{
    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(UserId.From(request.Id), cancellationToken);

        if (user is null)
        {
            return Error
                .NotFound("User not found")
                .ToResult<UpdateUserResponse>();
        }

        user.UpdateEmail(EmailAddress.From(request.Email));
        usersRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateUserResponse(request.Email));
    }
}
