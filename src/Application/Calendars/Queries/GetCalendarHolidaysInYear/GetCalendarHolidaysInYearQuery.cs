using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidaysInYear;

[Authorize(Permissions = SystemPermissions.CalendarHolidays.Read)]
public record GetCalendarHolidaysInYearQuery(Guid CalendarId, int Year) : IQuery<List<GetCalendarHolidaysInYearQueryResponse>>;
