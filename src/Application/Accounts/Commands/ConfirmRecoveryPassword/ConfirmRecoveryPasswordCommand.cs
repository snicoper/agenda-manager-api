using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Accounts.Commands.ConfirmRecoveryPassword;

public record ConfirmRecoveryPasswordCommand(string Token, string NewPassword, string ConfirmNewPassword)
    : ICommand;
