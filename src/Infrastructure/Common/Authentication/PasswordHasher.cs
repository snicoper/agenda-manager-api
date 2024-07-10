﻿using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.Utils;

namespace AgendaManager.Infrastructure.Common.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public Result<string> HashPassword(string password)
    {
        return !DomainRegex.StrongPassword().IsMatch(password)
            ? Error.Validation("Password", "Password is not strong enough").ToResult<string>()
            : Result.Success(BCrypt.Net.BCrypt.EnhancedHashPassword(password));
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
