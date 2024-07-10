﻿using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Infrastructure.Common.Authentication;

public class AuthenticationManager : IAuthenticationManager
{
    public Task<Result<TokenResult>> LoginAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TokenResult>> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
