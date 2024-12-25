using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Policies;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.Policies;

public class AppointmentConfirmationStrategyPolicyTests
{
    [Fact]
    public void DetermineInitialStatus_ShouldReturnPending_WhenRequireConfirmationIsProvided()
    {
        // Arrange
        var settings =
            CalendarSettingsFactory.CreateCalendarSettings(
                appointmentConfirmation: AppointmentConfirmationRequirementStrategy.Require);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        AppointmentConfirmationStrategyPolicy policy = new();

        // Act
        var result = policy.DetermineInitialStatus(calendar);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().Be(Result.Success(AppointmentStatus.Pending));
    }

    [Fact]
    public void DetermineInitialStatus_ShouldReturnAccepted_WhenAutoAcceptIsProvided()
    {
        // Arrange
        var settings =
            CalendarSettingsFactory.CreateCalendarSettings(
                appointmentConfirmation: AppointmentConfirmationRequirementStrategy.AutoAccept);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        AppointmentConfirmationStrategyPolicy policy = new();

        // Act
        var result = policy.DetermineInitialStatus(calendar);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().Be(Result.Success(AppointmentStatus.Accepted));
    }
}
