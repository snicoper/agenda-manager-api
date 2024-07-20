using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Commands.UpdateUser;

internal class UpdateUserCommandHandler(
    IUserRepository usersRepository,
    UserEmailService userEmailService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand, UpdateUserCommandResponse>
{
    public async Task<Result<UpdateUserCommandResponse>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(UserId.From(request.Id), cancellationToken);

        if (user is null)
        {
            return IdentityUserErrors.UserNotFound;
        }

        var validationResult = await userEmailService.UpdateUserEmailAsync(user, EmailAddress.From(request.Email));

        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<UpdateUserCommandResponse>();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateUserCommandResponse(request.Email));
    }
}
