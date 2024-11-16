using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Configurations.ValueObjects;

namespace AgendaManager.Domain.Configurations.Events;

public record ConfigurationCreatedDomainEvent(ConfigurationId ConfigurationId) : IDomainEvent;
