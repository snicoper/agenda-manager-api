using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Email, string Password) : IQuery<CreateUserResponse>;
