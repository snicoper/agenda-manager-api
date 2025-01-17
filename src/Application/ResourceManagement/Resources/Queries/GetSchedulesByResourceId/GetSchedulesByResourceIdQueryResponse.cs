using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.ResourceManagement.Resources.Enums;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceId;

public record GetSchedulesByResourceIdQueryResponse(
    Guid ScheduleId,
    Guid ResourceId,
    string Name,
    string? Description,
    bool IsActive,
    ResourceScheduleType Type,
    WeekDays AvailableDays,
    DateTimeOffset Start,
    DateTimeOffset End);
