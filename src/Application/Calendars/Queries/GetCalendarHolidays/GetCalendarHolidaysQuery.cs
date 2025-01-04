using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidays;

[Authorize(Permissions = SystemPermissions.CalendarHolidays.Read)]
public record GetCalendarHolidaysQuery(Guid CalendarId, int Year) : IQuery<List<GetCalendarHolidaysQueryResponse>>;
