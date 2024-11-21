using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.Entities;

namespace AgendaManager.Domain.Users.Events;

public record UserRoleRemovedDomainEvent(UserRole UserRole) : IDomainEvent;
