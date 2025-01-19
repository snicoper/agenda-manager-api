using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.ResourceManagement.Resources.Enums;

namespace AgendaManager.WebApi.Controllers.ResourceManagement.Resources.Contracts;

public record CreateScheduleRequest(
    string Name,
    string? Description,
    ResourceScheduleType Type,
    WeekDays AvailableDays,
    DateTimeOffset Start,
    DateTimeOffset End);
