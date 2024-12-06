using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

internal record UserProfileAddedDomainEvent(UserProfileId UserProfileId) : IDomainEvent;
