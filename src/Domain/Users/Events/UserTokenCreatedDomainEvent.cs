﻿using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Events;

public record UserTokenCreatedDomainEvent(UserTokenId UserTokenId) : IDomainEvent;
