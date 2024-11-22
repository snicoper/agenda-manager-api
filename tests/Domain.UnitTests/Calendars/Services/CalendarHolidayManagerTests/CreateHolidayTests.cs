using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.TestCommon.Factories;
using AgendaManager.TestCommon.Factories.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarHolidayManagerTests;

public class CreateHolidayTests : CalendarHolidayManagerTestsBase
{
    [Fact]
    public async Task Create_ShouldFailure_WhenKeyNotFound()
    {
        // Arrange
        const string selectedKey = CalendarConfigurationKeys.Holidays.ConflictOptions.AllowOverlapping;
        var (calendar, _) = GetCalendarWithConfiguration(selectedKey);

        SetupConfigurationRepositoryGetSelectedKey(null);

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarConfigurationErrors.KeyNotFound.FirstError());
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenConflictStrategyRejectIfOverlappingIsProvided()
    {
        // Arrange
        const string selectedKey = CalendarConfigurationKeys.Holidays.ConflictOptions.RejectIfOverlapping;
        var (calendar, configuration) = GetCalendarWithConfiguration(selectedKey);

        SetupConfigurationRepositoryGetSelectedKey(configuration);
        SetupAppointmentRepositoryGetOverlappingAppointments();

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenConflictStrategyCancelOverlappingIsProvided()
    {
        // Arrange
        const string selectedKey = CalendarConfigurationKeys.Holidays.ConflictOptions.CancelOverlapping;
        var (calendar, configuration) = GetCalendarWithConfiguration(selectedKey);

        SetupConfigurationRepositoryGetSelectedKey(configuration);
        var appointments = SetupAppointmentRepositoryGetOverlappingAppointments();

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        appointments.First().CurrentState.Value.Should().Be(AppointmentStatus.Cancelled);
    }

    [Fact]
    public async Task Create_ShouldSuccess_WhenConflictStrategyAllowOverlappingIsProvided()
    {
        // Arrange
        const string selectedKey = CalendarConfigurationKeys.Holidays.ConflictOptions.AllowOverlapping;
        var (calendar, configuration) = GetCalendarWithConfiguration(selectedKey);

        SetupConfigurationRepositoryGetSelectedKey(configuration);
        SetupAppointmentRepositoryGetOverlappingAppointments();
        SetupCalendarRepositoryGetByIdAsync(calendar);

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldSuccess_WhenAppointmentsIsEmpty()
    {
        // Arrange
        const string selectedKey = CalendarConfigurationKeys.Holidays.ConflictOptions.AllowOverlapping;
        var (calendar, configuration) = GetCalendarWithConfiguration(selectedKey);

        SetupConfigurationRepositoryGetSelectedKey(configuration);
        SetupAppointmentRepositoryGetOverlappingAppointments([]);
        SetupCalendarRepositoryGetByIdAsync(calendar);

        // Act
        var result = await SutCreateHolidayAsync(calendar.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    private static (Calendar Calendar, CalendarConfiguration Configuration) GetCalendarWithConfiguration(
        string selectedKey)
    {
        var calendar = CalendarFactory.CreateCalendar();
        var configurationResult = CalendarConfigurationFactory.CreateCalendarConfiguration(
            calendarId: calendar.Id,
            category: CalendarConfigurationKeys.Holidays.ConflictStrategy,
            selectedKey: selectedKey);

        return (calendar, configurationResult);
    }

    private async Task<Result<CalendarHoliday>> SutCreateHolidayAsync(CalendarId calendarId)
    {
        var result = await Sut.CreateHolidayAsync(
            calendarId: calendarId,
            period: PeriodFactory.Create(),
            weekDays: WeekDays.All,
            name: "Holiday",
            description: "Holiday description",
            cancellationToken: CancellationToken.None);

        return result;
    }

    private void SetupConfigurationRepositoryGetSelectedKey(CalendarConfiguration? configurationResult)
    {
        CalendarConfigurationRepository.GetBySelectedKeyAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(configurationResult);
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

    private void SetupCalendarRepositoryGetByIdAsync(Calendar calendarResult)
    {
        CalendarRepository.GetByIdAsync(calendarResult.Id, Arg.Any<CancellationToken>())
            .Returns(calendarResult);
    }
}
