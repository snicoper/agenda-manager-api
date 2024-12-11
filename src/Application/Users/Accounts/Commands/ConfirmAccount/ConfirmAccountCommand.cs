using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmAccount;

[AllowAnonymous]
public record ConfirmAccountCommand(string Token, string NewPassword, string ConfirmNewPassword) : ICommand;
