using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

public record RoleCreatedDomainEvent(RoleId RoleId) : IDomainEvent;
