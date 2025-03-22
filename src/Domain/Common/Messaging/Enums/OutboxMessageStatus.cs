namespace AgendaManager.Domain.Common.Messaging.Enums;

public enum OutboxMessageStatus
{
    Pending = 1,
    Published = 2,
    Failed = 3,
    PermanentlyFailed = 4,
}
