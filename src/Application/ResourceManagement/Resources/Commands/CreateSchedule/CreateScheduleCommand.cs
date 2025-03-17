using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.Common.WeekDays;
using AgendaManager.Domain.ResourceManagement.Resources.Enums;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.CreateSchedule;

[Authorize(Permissions = SystemPermissions.ResourceSchedules.Create)]
public record CreateScheduleCommand(
    Guid ResourceId,
    string Name,
    string? Description,
    ResourceScheduleType Type,
    WeekDays AvailableDays,
    DateTimeOffset Start,
    DateTimeOffset End)
    : ICommand<CreateScheduleCommandResponse>;
