﻿using AgendaManager.Application.Authentication.Models;
using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Authentication.Commands.Login;

[AllowAnonymous]
public record LoginCommand(string Email, string Password) : ICommand<TokenResult>;
