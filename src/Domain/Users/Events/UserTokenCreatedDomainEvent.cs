using System.Text.Json.Serialization;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users.Entities;

namespace AgendaManager.Domain.Users.Events;

public record UserTokenCreatedDomainEvent(UserToken UserToken) : IDomainEvent;
