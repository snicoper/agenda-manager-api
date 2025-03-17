namespace AgendaManager.Domain.Common.Messaging.Enums;

public enum OutboxStatus
{
    Pending = 1,
    Processed = 2,
    Failed = 3
}
