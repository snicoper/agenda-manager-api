using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.ResourceManagement.Resources.Enums;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceIdPaginated;

public record GetSchedulesByResourceIdPaginatedQueryResponse(
    Guid ScheduleId,
    Guid ResourceId,
    string Name,
    string? Description,
    bool IsActive,
    ResourceScheduleType Type,
    WeekDays AvailableDays,
    DateTimeOffset Start,
    DateTimeOffset End);
