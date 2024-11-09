using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserEmailManager
{
    Task<Result> UpdateUserEmailAsync(User user, EmailAddress newEmail);

    Task<Result> ValidateEmailAsync(User user, EmailAddress newEmail);
}
