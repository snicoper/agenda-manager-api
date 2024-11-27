using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmEmailResent;

public record ConfirmEmailResentCommand(string Email) : ICommand;
