using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace AgendaManager.Domain.UnitTests.Calendars.Services;

public class CalendarHolidayManagerTests
{
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICalendarSettingsRepository _calendarSettingsRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly CalendarHolidayManager _sut;

    public CalendarHolidayManagerTests()
    {
        _calendarRepository = Substitute.For<ICalendarRepository>();
        _calendarSettingsRepository = Substitute.For<ICalendarSettingsRepository>();
        _appointmentRepository = Substitute.For<IAppointmentRepository>();

        _sut = new CalendarHolidayManager(_calendarRepository, _calendarSettingsRepository, _appointmentRepository);
    }

    [Fact]
    public async Task CreateHoliday_ShouldSucceed_WhenOverlappingIsAllowed()
    {
        // Arrange
        var settingsResult = CalendarSettingsFactory.CreateCalendarSettings();
        var calendarResult = CalendarFactory.CreateCalendar();

        _calendarSettingsRepository.GetSettingsByCalendarIdAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(settingsResult);

        _appointmentRepository
            .GetOverlappingAppointmentsAsync(Arg.Any<CalendarId>(), Arg.Any<Period>(), Arg.Any<CancellationToken>())
            .Returns([]);

        _calendarRepository.GetByIdAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(calendarResult);

        // Act
        var result = await _sut.CreateHolidayAsync(
            settingsResult.CalendarId,
            PeriodFactory.Create(),
            WeekDays.Monday,
            "name",
            "description",
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateHoliday_ShouldFail_WhenOverlappingIsRejected()
    {
        // Arrange
        var appointmentResult = AppointmentFactory.Create();
        var settingsResult = CalendarSettingsFactory.CreateCalendarSettings();

        var errorResultExpected = CalendarHolidayErrors.CreateOverlappingReject.FirstError();

        _calendarSettingsRepository.GetSettingsByCalendarIdAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(settingsResult);

        _appointmentRepository
            .GetOverlappingAppointmentsAsync(Arg.Any<CalendarId>(), Arg.Any<Period>(), Arg.Any<CancellationToken>())
            .Returns([appointmentResult]);

        // Act
        var result = await _sut.CreateHolidayAsync(
            settingsResult.CalendarId,
            PeriodFactory.Create(),
            WeekDays.Monday,
            "name",
            "description",
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(errorResultExpected);
    }

    [Fact]
    public async Task CreateHoliday_ShouldFail_WhenCalendarNotFound()
    {
        // Arrange
        var settingsResult = CalendarSettingsFactory.CreateCalendarSettings();

        var errorResultExpected = CalendarErrors.CalendarNotFound.FirstError();

        _calendarSettingsRepository.GetSettingsByCalendarIdAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(settingsResult);

        _appointmentRepository
            .GetOverlappingAppointmentsAsync(Arg.Any<CalendarId>(), Arg.Any<Period>(), Arg.Any<CancellationToken>())
            .Returns([]);

        _calendarRepository.GetByIdAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _sut.CreateHolidayAsync(
            settingsResult.CalendarId,
            PeriodFactory.Create(),
            WeekDays.Monday,
            "name",
            "description",
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(errorResultExpected);
    }

    [Fact]
    public async Task CreateHoliday_ShouldFail_WhenSettingsNotFound()
    {
        // Arrange
        var errorResultExpected = CalendarSettingsErrors.CalendarSettingsNotFound.FirstError();

        _calendarSettingsRepository.GetSettingsByCalendarIdAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _sut.CreateHolidayAsync(
            CalendarId.Create(),
            PeriodFactory.Create(),
            WeekDays.Monday,
            "name",
            "description",
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(errorResultExpected);
    }
}
