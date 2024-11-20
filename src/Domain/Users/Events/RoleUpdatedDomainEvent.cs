using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Users.Events;

public record RoleUpdatedDomainEvent(RoleId RoleId) : IDomainEvent;
