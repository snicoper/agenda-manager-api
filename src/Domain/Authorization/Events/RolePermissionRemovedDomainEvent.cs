using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Authorization.Events;

public record RolePermissionRemovedDomainEvent(RoleId RoleId, PermissionId PermissionId) : IDomainEvent;
