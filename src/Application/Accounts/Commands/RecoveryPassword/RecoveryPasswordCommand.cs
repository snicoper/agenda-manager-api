using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Accounts.Commands.RecoveryPassword;

public record RecoveryPasswordCommand(string Email) : ICommand;
