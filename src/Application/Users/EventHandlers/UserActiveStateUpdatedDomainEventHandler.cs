using AgendaManager.Application.Common.Abstractions;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.Interfaces;
using AgendaManager.Domain.AuditRecords.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Users.EventHandlers;

public class UserActiveStateUpdatedDomainEventHandler(
    ILogger<BaseEventHandler<UserActiveStateUpdatedDomainEvent>> logger,
    IAuditRecordRepository auditRecordRepository,
    IUnitOfWork unitOfWork)
    : BaseEventHandler<UserActiveStateUpdatedDomainEvent>(logger)
{
    protected override async Task HandleEvent(
        UserActiveStateUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var domain = typeof(User);

        var auditRecord = AuditRecord.Create(
            id: AuditRecordId.Create(),
            aggregateId: nameof(User),
            namespaceName: domain.Namespace!,
            aggregateName: domain.Name,
            propertyName: nameof(User.IsActive),
            oldValue: (!notification.State).ToString(),
            newValue: notification.State.ToString(),
            actionType: AuditRecordActionType.Update);

        await auditRecordRepository.AddAsync(auditRecord, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
