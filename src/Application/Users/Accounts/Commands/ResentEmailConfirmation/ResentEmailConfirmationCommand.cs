using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.ResentEmailConfirmation;

[AllowAnonymous]
public record ResentEmailConfirmationCommand(string Email) : ICommand;
