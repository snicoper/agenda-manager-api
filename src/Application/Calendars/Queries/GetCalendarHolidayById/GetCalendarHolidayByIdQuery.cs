using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidayById;

[Authorize(Permissions = SystemPermissions.CalendarHolidays.Read)]
public record GetCalendarHolidayByIdQuery(Guid CalendarId, Guid CalendarHolidayId)
    : IQuery<GetCalendarHolidayByIdQueryResponse>;
