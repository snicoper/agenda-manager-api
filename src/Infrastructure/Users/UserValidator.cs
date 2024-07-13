using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users;

public class UserValidator(AppDbContext context) : IUserValidator
{
    public async Task<Result> IsUniqueEmail(EmailAddress email)
    {
        return await context.Users
            .AnyAsync(user => user.Email.Equals(email))
            ? Error.Validation(nameof(User.Email), "Email already exists")
            : Result.Success();
    }
}
