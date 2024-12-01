using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmEmailResent;

[AllowAnonymous]
public record ConfirmEmailResentCommand(string Email) : ICommand;
