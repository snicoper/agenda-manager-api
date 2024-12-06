using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

internal record UserProfileUpdatedDomainEvent(UserId UserId) : IDomainEvent;
