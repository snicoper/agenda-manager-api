﻿using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Users.Events;

public record RolePermissionRemovedDomainEvent(RoleId RoleId, PermissionId PermissionId) : IDomainEvent;
