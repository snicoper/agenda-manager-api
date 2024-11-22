using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Policies;
using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
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
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.Appointments.ConfirmationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation);

        List<CalendarConfiguration> configurations = [configuration];
        AppointmentConfirmationStrategyPolicy policy = new();

        // Act
        var result = policy.DetermineInitialStatus(configurations);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().Be(Result.Success(AppointmentStatus.Pending));
    }

    [Fact]
    public void DetermineInitialStatus_ShouldReturnAccepted_WhenDirectIsProvided()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.Appointments.ConfirmationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.AutoAccept);

        List<CalendarConfiguration> configurations = [configuration];
        AppointmentConfirmationStrategyPolicy policy = new();

        // Act
        var result = policy.DetermineInitialStatus(configurations);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().Be(Result.Success(AppointmentStatus.Accepted));
    }

    [Fact]
    public void DetermineInitialStatus_ShouldReturnError_WhenNoCreationStrategyIsProvided()
    {
        // Arrange
        List<CalendarConfiguration> configurations = [];
        AppointmentConfirmationStrategyPolicy policy = new();

        // Act
        var result = policy.DetermineInitialStatus(configurations);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.FirstError().Should().Be(AppointmentErrors.MissingCreationStrategy.FirstError());
    }
}
