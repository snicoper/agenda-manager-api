using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Events;

public record ServiceCreatedDomainEvent(ServiceId ServiceId) : IDomainEvent;
