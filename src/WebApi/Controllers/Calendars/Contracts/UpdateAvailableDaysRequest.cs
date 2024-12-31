using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.WebApi.Controllers.Calendars.Contracts;

public record UpdateAvailableDaysRequest(WeekDays AvailableDays);
