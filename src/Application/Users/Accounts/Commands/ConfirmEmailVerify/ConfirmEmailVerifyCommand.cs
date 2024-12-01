using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmEmailVerify;

[AllowAnonymous]
public record ConfirmEmailVerifyCommand(string Token) : ICommand;
