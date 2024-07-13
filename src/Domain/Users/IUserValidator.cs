using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public interface IUserValidator
{
    Task<Result> IsUniqueEmail(EmailAddress email);
}
