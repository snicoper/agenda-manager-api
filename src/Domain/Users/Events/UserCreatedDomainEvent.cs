using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

public class UserCreatedDomainEvent(UserId userId) : DomainEvent
{
    public UserId UserId { get; private set; } = userId;
}
