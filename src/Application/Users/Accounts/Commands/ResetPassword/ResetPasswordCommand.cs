using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.ResetPassword;

[AllowAnonymous]
public record ResetPasswordCommand(string Token, string NewPassword, string ConfirmNewPassword)
    : ICommand;
