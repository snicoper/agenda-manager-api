using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.RecoveryPassword;

[AllowAnonymous]
public record RecoveryPasswordCommand(string Email) : ICommand;
