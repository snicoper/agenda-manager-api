using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmRecoveryPassword;

[AllowAnonymous]
public record ConfirmRecoveryPasswordCommand(string Token, string NewPassword, string ConfirmNewPassword)
    : ICommand;
