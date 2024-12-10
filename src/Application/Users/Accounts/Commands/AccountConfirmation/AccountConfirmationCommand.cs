using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.AccountConfirmation;

[AllowAnonymous]
public record AccountConfirmationCommand(string Token, string NewPassword, string ConfirmNewPassword) : ICommand;
