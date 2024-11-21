using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.Entities;

namespace AgendaManager.Domain.Users.Events;

public record UserRoleAddedDomainEvent(UserRole UserRole) : IDomainEvent;
