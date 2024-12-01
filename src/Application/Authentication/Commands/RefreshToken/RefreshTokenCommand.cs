using AgendaManager.Application.Authentication.Models;
using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Authentication.Commands.RefreshToken;

[AllowAnonymous]
public record RefreshTokenCommand(string RefreshToken) : ICommand<TokenResult>;
