using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Authorization.Events;

public record RoleCreatedDomainEvent(RoleId RoleId) : IDomainEvent;
