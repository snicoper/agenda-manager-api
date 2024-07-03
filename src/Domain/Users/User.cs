using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class User : AuditableEntity
{
    private User()
    {
    }

    private User(UserId userId)
    {
        Id = userId;
    }

    public UserId Id { get; private set; } = default!;
}
