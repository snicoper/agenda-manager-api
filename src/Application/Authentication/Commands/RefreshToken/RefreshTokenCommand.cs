using AgendaManager.Application.Authentication.Models;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<TokenResult>;
