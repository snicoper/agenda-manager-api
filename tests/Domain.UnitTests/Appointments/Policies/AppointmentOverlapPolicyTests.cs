using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.Policies;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Factories;
using AgendaManager.TestCommon.Factories.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Appointments.Policies;

public class AppointmentOverlapPolicyTests
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly AppointmentOverlapPolicy _sut;

    public AppointmentOverlapPolicyTests()
    {
        _appointmentsRepository = Substitute.For<IAppointmentRepository>();
        _sut = new AppointmentOverlapPolicy(_appointmentsRepository);
    }

    [Fact]
    public async Task IsOverlapping_ShouldReturnTrue_WhenAppointmentOverlappingTypeIsAllow()
    {
        // Arrange
        var settings =
            CalendarSettingsFactory.CreateCalendarSettings(
                appointmentOverlapping: AppointmentOverlappingStrategy.Allow);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);

        _appointmentsRepository.IsOverlappingAppointmentsAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsOverlappingAsync(
            calendar,
            PeriodFactory.Create(),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IsOverlapping_ShouldReturnTrue_WhenAppointmentOverlappingTypeIsReject()
    {
        // Arrange
        var settings =
            CalendarSettingsFactory.CreateCalendarSettings(
                appointmentOverlapping: AppointmentOverlappingStrategy.Reject);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);

        _appointmentsRepository.IsOverlappingAppointmentsAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsOverlappingAsync(
            calendar,
            PeriodFactory.Create(),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
