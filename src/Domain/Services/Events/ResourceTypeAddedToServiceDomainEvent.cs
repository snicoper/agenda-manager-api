using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Events;

public record ResourceTypeAddedToServiceDomainEvent(ServiceId ServiceId, ResourceTypeId ResourceTypeId) : IDomainEvent;
