using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Commands.VerifyEmail;

[AllowAnonymous]
public record VerifyEmailCommand(string Token) : ICommand;
