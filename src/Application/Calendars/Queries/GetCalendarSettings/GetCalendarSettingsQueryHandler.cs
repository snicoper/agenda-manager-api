using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarSettings;

internal class GetCalendarSettingsQueryHandler(ICalendarRepository calendarRepository)
    : IQueryHandler<GetCalendarSettingsQuery, GetCalendarSettingsQueryResponse>
{
    public async Task<Result<GetCalendarSettingsQueryResponse>> Handle(
        GetCalendarSettingsQuery request,
        CancellationToken cancellationToken)
    {
        // Get the calendar by id and check if exists.
        var calendar = await calendarRepository.GetByIdWithSettingsAsync(
            CalendarId.From(request.CalendarId),
            cancellationToken);

        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // Map the calendar settings to the response.
        var response = new GetCalendarSettingsQueryResponse(
            CalendarId: calendar.Id.Value,
            TimeZone: calendar.Settings.TimeZone.Value,
            AppointmentConfirmationRequirement: calendar.Settings.AppointmentConfirmationRequirement,
            AppointmentOverlapping: calendar.Settings.AppointmentOverlapping,
            HolidayConflict: calendar.Settings.HolidayConflict,
            ResourceScheduleValidation: calendar.Settings.ResourceScheduleValidation);

        return Result.Success(response);
    }
}
