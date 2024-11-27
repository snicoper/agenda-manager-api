using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Accounts.Commands.ConfirmEmailVerify;

public record ConfirmEmailVerifyCommand(string Token) : ICommand;
