using AgendaManager.Domain.AuditRecords.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.AuditRecords.Events;

public record AuditRecordCreatedDomainEvent(AuditRecordId AuditRecordId) : IDomainEvent;
