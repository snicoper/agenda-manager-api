﻿using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

public record UserRoleRemovedDomainEvent(UserId UserId, RoleId RoleId) : IDomainEvent;
