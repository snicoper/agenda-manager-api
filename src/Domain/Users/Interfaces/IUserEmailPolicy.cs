using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserEmailPolicy
{
    Task<Result> ValidateEmailAsync(User user, EmailAddress newEmail);
}
