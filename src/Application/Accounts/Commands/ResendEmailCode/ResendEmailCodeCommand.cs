using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Accounts.Commands.ResendEmailCode;

public record ResendEmailCodeCommand(string Email) : ICommand;
