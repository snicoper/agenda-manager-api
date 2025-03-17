using AgendaManager.Domain.Common.WeekDays;

namespace AgendaManager.WebApi.Controllers.Calendars.Contracts;

public record UpdateAvailableDaysRequest(WeekDays AvailableDays);
