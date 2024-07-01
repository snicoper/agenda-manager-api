using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Common.Interfaces;

public interface IDomainEvent
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    void AddDomainEvent(DomainEvent domainEvent);

    void RemoveDomainEvent(DomainEvent domainEvent);

    void ClearDomainEvents();
}
