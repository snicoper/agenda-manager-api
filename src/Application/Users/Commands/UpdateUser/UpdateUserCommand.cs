using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid UserId, string Email) : ICommand<UpdateUserCommandResponse>;
