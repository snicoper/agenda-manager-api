using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Models.Users;

namespace AgendaManager.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<TokenResult>;
