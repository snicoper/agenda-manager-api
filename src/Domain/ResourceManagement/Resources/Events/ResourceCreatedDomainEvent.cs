﻿using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.Resources.Events;

public record ResourceCreatedDomainEvent(ResourceId ResourceId) : IDomainEvent;
