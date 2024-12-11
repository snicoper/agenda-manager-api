using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.RequestPasswordReset;

[AllowAnonymous]
public record RequestPasswordResetCommand(string Email) : ICommand;
