using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WeekDays;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarManagerTests;

public class CalendarManagerUpdateAvailableDaysTests : CalendarManagerTestsBase
{
    /// <summary>
    /// Comprueba que se actualicen los días disponibles de un calendario.
    /// </summary>
    [Fact]
    public async Task UpdateAvailableDays_ShouldReturnSucceed_WhenValidParametersProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarRepositoryGetByIdWithSettings(calendar);
        SetupAppointmentRepositoryGetOverlappingAppointmentsByWeekDays([]);

        // Act
        var result = await Sut.UpdateAvailableDaysAsync(
            calendarId: calendar.Id,
            availableDays: WeekDays.Monday | WeekDays.Tuesday,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    /// <summary>
    /// Comprueba que se devuelva un error cuando no se encuentra el calendario.
    /// </summary>
    [Fact]
    public async Task UpdateAvailableDays_ShouldReturnFailure_WhenCalendarNotFound()
    {
        // Arrange
        SetupCalendarRepositoryGetByIdWithSettings();

        // Act
        var result = await Sut.UpdateAvailableDaysAsync(
            calendarId: CalendarId.Create(),
            availableDays: WeekDays.Monday | WeekDays.Tuesday,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarErrors.CalendarNotFound.FirstError());
    }

    /// <summary>
    /// Comprueba que se devuelva un error cuando hay citas superpuestas y la estrategia es rechazar.
    /// </summary>
    [Fact]
    public async Task UpdateAvailableDays_ShouldReturnFailure_WhenOverlappingAppointmentsAndRejectStrategy()
    {
        // Arrange
        var calendarSettings = CalendarSettingsFactory.CreateCalendarSettings(
            holidayConflict: HolidayConflictStrategy.Reject);
        var calendar = CalendarFactory.CreateCalendar(settings: calendarSettings, availableDays: WeekDays.Friday);
        SetupCalendarRepositoryGetByIdWithSettings(calendar);

        var appointments = CreateAppointmentsForNextMonday();
        SetupAppointmentRepositoryGetOverlappingAppointmentsByWeekDays(appointments);

        // Act
        var result = await Sut.UpdateAvailableDaysAsync(
            calendarId: calendar.Id,
            availableDays: WeekDays.Monday | WeekDays.Tuesday,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarHolidayErrors.CreateOverlappingReject.FirstError());
    }

    /// <summary>
    /// Comprueba que se actualicen los días disponibles de un calendario cuando hay citas superpuestas
    /// y la estrategia es cancelar.
    /// </summary>
    [Fact]
    public async Task UpdateAvailableDays_ShouldReturnSucceed_WhenOverlappingAppointmentsAndCancelStrategy()
    {
        // Arrange
        var calendarSettings = CalendarSettingsFactory.CreateCalendarSettings(
            holidayConflict: HolidayConflictStrategy.Cancel);
        var calendar = CalendarFactory.CreateCalendar(settings: calendarSettings, availableDays: WeekDays.Friday);
        SetupCalendarRepositoryGetByIdWithSettings(calendar);

        var appointments = CreateAppointmentsForNextMonday();
        SetupAppointmentRepositoryGetOverlappingAppointmentsByWeekDays(appointments);

        // Act
        var result = await Sut.UpdateAvailableDaysAsync(
            calendarId: calendar.Id,
            availableDays: WeekDays.Monday | WeekDays.Tuesday,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        appointments.First().CurrentState.Value.Should().Be(AppointmentStatus.Cancelled);
    }

    /// <summary>
    /// Comprueba que se actualicen los días disponibles de un calendario cuando hay citas superpuestas
    /// si la estrategia es permitir.
    /// </summary>
    [Fact]
    public async Task UpdateAvailableDays_ShouldReturnSuccess_WhenOverlappingAppointmentsAndAllowStrategy()
    {
        // Arrange
        var calendarSettings = CalendarSettingsFactory.CreateCalendarSettings(
            holidayConflict: HolidayConflictStrategy.Allow);
        var calendar = CalendarFactory.CreateCalendar(settings: calendarSettings, availableDays: WeekDays.Monday);
        SetupCalendarRepositoryGetByIdWithSettings(calendar);

        var appointments = CreateAppointmentsForNextMonday();
        SetupAppointmentRepositoryGetOverlappingAppointmentsByWeekDays(appointments);

        // Act
        var result = await Sut.UpdateAvailableDaysAsync(
            calendarId: calendar.Id,
            availableDays: WeekDays.Monday | WeekDays.Tuesday,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    /// <summary>
    /// Comprueba que se devuelva un error cuando hay citas superpuestas y la estrategia es desconocida.
    /// </summary>
    [Fact]
    public async Task UpdateAvailableDays_ShouldReturnFailure_WhenOverlappingAppointmentsAndUnknownStrategy()
    {
        // Arrange
        var calendarSettings = CalendarSettingsFactory.CreateCalendarSettings(
            holidayConflict: (HolidayConflictStrategy)99);
        var calendar = CalendarFactory.CreateCalendar(settings: calendarSettings, availableDays: WeekDays.Friday);
        SetupCalendarRepositoryGetByIdWithSettings(calendar);

        var appointments = CreateAppointmentsForNextMonday();
        SetupAppointmentRepositoryGetOverlappingAppointmentsByWeekDays(appointments);

        // Act
        Func<Task> act = async () => await Sut.UpdateAvailableDaysAsync(
            calendarId: calendar.Id,
            availableDays: WeekDays.Monday | WeekDays.Tuesday,
            cancellationToken: CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    private static List<Appointment> CreateAppointmentsForNextMonday()
    {
        // Next Monday.
        var mondayNext = DateTimeOffset.UtcNow.AddDays(7 - (int)DateTimeOffset.UtcNow.DayOfWeek + 1);
        var period = Period.From(mondayNext, mondayNext.AddHours(1));
        List<Appointment> appointments =
        [
            AppointmentFactory.CreateAppointment(period: period).Value!,
            AppointmentFactory.CreateAppointment(period: period).Value!
        ];

        return appointments;
    }

    private void SetupCalendarRepositoryGetByIdWithSettings(Calendar? calendar = null)
    {
        CalendarRepository.GetByIdWithSettingsAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<CancellationToken>())
            .Returns(calendar);
    }

    private void SetupAppointmentRepositoryGetOverlappingAppointmentsByWeekDays(List<Appointment> returnValue)
    {
        AppointmentRepository.GetOverlappingAppointmentsByWeekDaysAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<WeekDays>(),
                Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }
}
