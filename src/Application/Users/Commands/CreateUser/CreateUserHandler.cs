using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Commands.CreateUser;

internal class CreateUserHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    : IQueryHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await usersRepository.GetByEmailAsync(Email.From(request.Email), cancellationToken);

        if (existingUser is not null)
        {
            return Error.Conflict("The Email already exists").ToResult<CreateUserResponse>();
        }

        var newUser = User.Create(
            UserId.Create(),
            Email.From("test2@example.com"),
            "test2",
            "peric2o",
            "palote2");

        await usersRepository.AddAsync(newUser, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Create(new CreateUserResponse(newUser.Id.Value));
    }
}
