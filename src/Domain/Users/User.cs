using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AgendaManager.Domain.Users;

public sealed class User : IdentityUser<Guid>, IDomainEvent
{
    private readonly List<DomainEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
