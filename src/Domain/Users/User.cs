using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AgendaManager.Domain.Users;

public sealed class User : IdentityUser<Guid>, IDomainEvent
{
    private readonly List<DomainEvent> _domainEvents = [];

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? RefreshToken { get; set; }

    public bool Active { get; set; }

    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }

    public DateTimeOffset EntryDate { get; set; }

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
