﻿using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Policies;
using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.Policies;

public class AppointmentCreationStrategyPolicyTests
{
    [Fact]
    public void DetermineInitialStatus_ShouldReturnPending_WhenRequireConfirmationIsProvided()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            id: CalendarConfigurationId.Create(),
            calendarId: CalendarId.Create(),
            category: CalendarConfigurationKeys.Appointments.CreationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.CreationOptions.RequireConfirmation);

        var configurations = new List<CalendarConfiguration> { configuration };

        var policy = new AppointmentCreationStrategyPolicy();

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
            id: CalendarConfigurationId.Create(),
            calendarId: CalendarId.Create(),
            category: CalendarConfigurationKeys.Appointments.CreationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.CreationOptions.Direct);

        var configurations = new List<CalendarConfiguration> { configuration };

        var policy = new AppointmentCreationStrategyPolicy();

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
        var configurations = new List<CalendarConfiguration>();
        var policy = new AppointmentCreationStrategyPolicy();

        // Act
        var result = policy.DetermineInitialStatus(configurations);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.FirstError().Should().Be(AppointmentErrors.MissingCreationStrategy.FirstError());
    }
}
