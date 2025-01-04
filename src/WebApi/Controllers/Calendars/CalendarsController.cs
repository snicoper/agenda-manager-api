using AgendaManager.Application.Calendars.Commands.CreateCalendar;
using AgendaManager.Application.Calendars.Commands.CreateCalendarHoliday;
using AgendaManager.Application.Calendars.Commands.ToggleIsActive;
using AgendaManager.Application.Calendars.Commands.UpdateAvailableDays;
using AgendaManager.Application.Calendars.Commands.UpdateCalendar;
using AgendaManager.Application.Calendars.Commands.UpdateCalendarSettings;
using AgendaManager.Application.Calendars.Queries.GetCalendarById;
using AgendaManager.Application.Calendars.Queries.GetCalendarHolidayById;
using AgendaManager.Application.Calendars.Queries.GetCalendarHolidaysInYear;
using AgendaManager.Application.Calendars.Queries.GetCalendarSettings;
using AgendaManager.Application.Calendars.Queries.GetCalendarsPaginated;
using AgendaManager.Application.Common.Http;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Controllers.Calendars.Contracts;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Calendars;

[Route("api/v{version:apiVersion}/calendars")]
public class CalendarsController : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of calendars.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetCalendarsPaginatedQueryResponse>>>> GetCalendarsPaginated(
        [FromQuery] RequestData requestData)
    {
        var query = new GetCalendarsPaginatedQuery(requestData);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Get a calendar by id.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{calendarId:guid}")]
    public async Task<ActionResult<Result<GetCalendarByIdQueryResponse>>> GetCalendarById(Guid calendarId)
    {
        var query = new GetCalendarByIdQuery(calendarId);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Get the settings of a calendar.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{calendarId:guid}/settings")]
    public async Task<ActionResult<Result<GetCalendarSettingsQueryResponse>>> GetCalendarSettings(Guid calendarId)
    {
        var query = new GetCalendarSettingsQuery(calendarId);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Get holiday by id.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{calendarId:guid}/holidays/{calendarHolidayId:guid}")]
    public async Task<ActionResult<Result<GetCalendarHolidayByIdQueryResponse>>> GetCalendarHolidayById(
        Guid calendarId,
        Guid calendarHolidayId)
    {
        var query = new GetCalendarHolidayByIdQuery(calendarId, calendarHolidayId);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Get list of holidays for a calendar.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{calendarId:guid}/holidays/year/{year:int}")]
    public async Task<ActionResult<Result<List<GetCalendarHolidaysInYearQueryResponse>>>> GetCalendarHolidaysInYear(
        Guid calendarId,
        int year)
    {
        var query = new GetCalendarHolidaysInYearQuery(calendarId, year);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new calendar.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<Result<CreateCalendarCommandResponse>>> CreateCalendar(CreateCalendarRequest request)
    {
        var command = new CreateCalendarCommand(request.Name, request.Description, request.IanaTimeZone);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new holiday for a calendar.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("{calendarId:guid}/holidays")]
    public async Task<ActionResult<Result<CreateCalendarHolidayCommandResponse>>> CreateCalendarHoliday(
        Guid calendarId,
        CreateCalendarHolidayCommandRequest request)
    {
        var command = new CreateCalendarHolidayCommand(calendarId, request.Start, request.End, request.Name);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Update a calendar.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{calendarId:guid}")]
    public async Task<ActionResult<Result>> UpdateCalendar(Guid calendarId, UpdateCalendarRequest request)
    {
        var command = new UpdateCalendarCommand(calendarId, request.Name, request.Description);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Update the AvailableDays of a calendar.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{calendarId:guid}/available-days")]
    public async Task<ActionResult<Result>> UpdateAvailableDays(
        Guid calendarId,
        UpdateAvailableDaysRequest request)
    {
        var command = new UpdateAvailableDaysCommand(calendarId, request.AvailableDays);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Update the settings of a calendar.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{calendarId:guid}/settings")]
    public async Task<ActionResult<Result>> UpdateCalendarSettings(
        Guid calendarId,
        UpdateCalendarSettingsRequest request)
    {
        var command = new UpdateCalendarSettingsCommand(
            CalendarId: calendarId,
            TimeZone: request.TimeZone,
            AppointmentConfirmationRequirement: request.AppointmentConfirmationRequirement,
            AppointmentOverlapping: request.AppointmentOverlapping,
            HolidayConflict: request.HolidayConflict,
            ResourceScheduleValidation: request.ResourceScheduleValidation);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Toggle the IsActive property of a calendar.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{calendarId:guid}/toggle-is-active")]
    public async Task<ActionResult<Result>> ToggleIsActive(Guid calendarId)
    {
        var command = new ToggleIsActiveCommand(calendarId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
