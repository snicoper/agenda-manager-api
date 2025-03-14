﻿using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

public record UserEmailUpdatedDomainEvent(UserId UserId) : IDomainEvent;
