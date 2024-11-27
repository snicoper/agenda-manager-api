using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.RecoveryPassword;

public record RecoveryPasswordCommand(string Email) : ICommand;
