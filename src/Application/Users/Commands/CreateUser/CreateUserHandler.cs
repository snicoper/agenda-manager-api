using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Commands.CreateUser;

internal class CreateUserHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            UserId.Create(),
            Email.From("test2@example.com"),
            "test2",
            "peric2o",
            "palote2");

        var userCreatedResult = await usersRepository.CreateAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create(userCreatedResult.Id.Value);
    }
}
