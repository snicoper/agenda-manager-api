using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

public record UserAssignableResourceUpdatedDomainEvent(UserId Id, bool IsAssignableResource) : IDomainEvent;
