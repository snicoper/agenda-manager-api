using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Factories;
using AgendaManager.TestCommon.Factories.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarHolidayManagerTests;

public class CreateHolidayTests : CalendarHolidayManagerTestsBase
{
    [Fact]
    public async Task CreateHoliday_ShouldReturnHoliday_WhenValidValuesAreProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(false);
        SetupCalendarHolidayRepositoryIsOverlappingInPeriodByCalendarIdAsync(false);
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);
        SetupAppointmentRepositoryGetOverlappingAppointments([]);

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateHoliday_ShouldReturnError_WhenHolidayNameExists()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(true);

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarHolidayErrors.NameAlreadyExists.FirstError());
    }

    [Fact]
    public async Task CreateHoliday_ShouldReturnError_WhenOverlappingHoliday()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(false);
        SetupCalendarHolidayRepositoryIsOverlappingInPeriodByCalendarIdAsync(true);

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarHolidayErrors.HolidaysOverlap.FirstError());
    }

    [Fact]
    public async Task CreateHoliday_ShouldReturnError_WhenCalendarNotFound()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(false);
        SetupCalendarHolidayRepositoryIsOverlappingInPeriodByCalendarIdAsync(false);
        SetupCalendarRepositoryGetByIdWithSettingsAsync(null);

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarErrors.CalendarNotFound.FirstError());
    }

    [Fact]
    public async Task CreateHoliday_ShouldReturnError_WhenOverlappingAppointments()
    {
        // Arrange
        var settings = CalendarSettingsFactory.CreateCalendarSettings(holidayConflict: HolidayConflictStrategy.Reject);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(false);
        SetupCalendarHolidayRepositoryIsOverlappingInPeriodByCalendarIdAsync(false);
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);
        SetupAppointmentRepositoryGetOverlappingAppointments();

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarHolidayErrors.CreateOverlappingReject.FirstError());
    }

    [Fact]
    public async Task CreateHoliday_ShouldReturnHoliday_WhenOverlappingAppointmentsAndCancel()
    {
        // Arrange
        var settings = CalendarSettingsFactory.CreateCalendarSettings(holidayConflict: HolidayConflictStrategy.Cancel);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(false);
        SetupCalendarHolidayRepositoryIsOverlappingInPeriodByCalendarIdAsync(false);
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);

        // Act
        var appointments = SetupAppointmentRepositoryGetOverlappingAppointments();
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        appointments.First().CurrentState.Value.Should().Be(AppointmentStatus.Cancelled);
    }

    [Fact]
    public async Task CreateHoliday_ShouldReturnHoliday_WhenOverlappingAppointmentsAndAllow()
    {
        // Arrange
        var settings = CalendarSettingsFactory.CreateCalendarSettings(holidayConflict: HolidayConflictStrategy.Allow);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(false);
        SetupCalendarHolidayRepositoryIsOverlappingInPeriodByCalendarIdAsync(false);
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);
        SetupAppointmentRepositoryGetOverlappingAppointments();

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    private async Task<Result<CalendarHoliday>> SutCreateHolidayAsync(CalendarId calendarId)
    {
        var result = await Sut.CreateHolidayAsync(
            calendarId: calendarId,
            period: PeriodFactory.Create(),
            name: "Holiday",
            cancellationToken: CancellationToken.None);

        return result;
    }

    private void SetupCalendarHolidayRepositoryExistsHolidayNameAsync(bool exists)
    {
        CalendarHolidayRepository.ExistsHolidayNameAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<CalendarHolidayId>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(exists);
    }

    private void SetupCalendarHolidayRepositoryIsOverlappingInPeriodByCalendarIdAsync(bool exists)
    {
        CalendarHolidayRepository.IsOverlappingInPeriodByCalendarIdAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(exists);
    }

    private void SetupCalendarRepositoryGetByIdWithSettingsAsync(Calendar? calendarResult)
    {
        CalendarRepository.GetByIdWithSettingsAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(calendarResult);
    }

    private List<Appointment> SetupAppointmentRepositoryGetOverlappingAppointments(
        List<Appointment>? appointmentsResult = null)
    {
        // Overlap with a holiday will always result in a positive result.
        var period = PeriodFactory.Create(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);

        appointmentsResult ??=
        [
            AppointmentFactory.CreateAppointment(period: period).Value!,
            AppointmentFactory.CreateAppointment(period: period).Value!
        ];

        AppointmentRepository.GetOverlappingAppointmentsAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(appointmentsResult);

        return appointmentsResult;
    }
}
