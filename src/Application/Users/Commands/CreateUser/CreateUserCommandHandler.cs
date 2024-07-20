using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Commands.CreateUser;

internal class CreateUserCommandHandler(IUserRepository usersRepository, IUnitOfWork unitOfWork)
    : IQueryHandler<CreateUserCommand, CreateUserCommandResponse>
{
    public async Task<Result<CreateUserCommandResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await usersRepository.GetByEmailAsync(EmailAddress.From(request.Email), cancellationToken);

        if (existingUser is not null)
        {
            return IdentityUserErrors.EmailAlreadyExists;
        }

        var newUser = User.Create(
            UserId.Create(),
            EmailAddress.From("test2@example.com"),
            "peric2o",
            "palote2",
            "password2");

        await usersRepository.AddAsync(newUser, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Create(new CreateUserCommandResponse(newUser.Id.Value));
    }
}
