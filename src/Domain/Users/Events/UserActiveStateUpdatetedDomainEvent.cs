using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

public record UserActiveStateUpdatetedDomainEvent(UserId UserId, bool State) : IDomainEvent;
