using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceAvailabilities.ValueObjects;

namespace AgendaManager.Domain.ResourceAvailabilities.Events;

public record ResourceAvailabilityCreatedDomainEvent(ResourceAvailabilityId ResourceAvailabilityId) : IDomainEvent;
