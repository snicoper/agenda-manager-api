using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserEmailPolicy
{
    Task<Result> ValidateEmailAsync(User user, EmailAddress newEmail);
}
