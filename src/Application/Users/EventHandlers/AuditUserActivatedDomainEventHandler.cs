﻿using AgendaManager.Application.Common.Abstractions;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.Interfaces;
using AgendaManager.Domain.AuditRecords.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Users.EventHandlers;

public class AuditUserActivatedDomainEventHandler(
    ILogger<BaseEventHandler<UserActivatedDomainEvent>> logger,
    IAuditRecordRepository auditRecordRepository,
    IUnitOfWork unitOfWork)
    : BaseEventHandler<UserActivatedDomainEvent>(logger)
{
    protected override async Task HandleEvent(
        UserActivatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var domain = typeof(User);

        // Create the audit record.
        var auditRecord = AuditRecord.Create(
            id: AuditRecordId.Create(),
            aggregateId: nameof(User),
            namespaceName: domain.Namespace!,
            aggregateName: domain.Name,
            propertyName: nameof(User.IsActive),
            oldValue: false.ToString(),
            newValue: true.ToString(),
            actionType: AuditRecordActionType.Update);

        // Add and save the audit record.
        await auditRecordRepository.AddAsync(auditRecord, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
