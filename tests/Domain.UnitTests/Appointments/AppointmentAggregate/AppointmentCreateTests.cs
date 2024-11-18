using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.AppointmentAggregate;

public class AppointmentCreateTests
{
    [Fact]
    public void Create_ShouldSuccess_WithValidValues()
    {
        // Act
        var appointment = AppointmentFactory.CreateAppointment();

        // Assert
        appointment.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_ShouldRaiseEvent_WithValidValues()
    {
        // Act
        var appointment = AppointmentFactory.CreateAppointment();

        // Assert
        appointment.Value!.DomainEvents.Should().Contain(x => x is AppointmentCreatedDomainEvent);
    }

    [Fact]
    public void Create_ShouldRaiseStatusEvent_WithValidValues()
    {
        // Act
        var appointment = AppointmentFactory.CreateAppointment();

        // Assert
        appointment.Value!.DomainEvents.Should().Contain(x => x is AppointmentStatusChangedDomainEvent);
    }

    [Fact]
    public void Create_ShouldResultTypeCreated_WithValidValues()
    {
        // Act
        var appointment = AppointmentFactory.CreateAppointment();

        // Assert
        appointment.ResultType.Should().Be(ResultType.Created);
    }

    [Theory]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.RequiresRescheduling)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public void Create_ShouldFail_WhenInvalidStatus(AppointmentStatus status)
    {
        // Act
        var appointment = AppointmentFactory.CreateAppointment(status: status);

        // Assert
        appointment.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_ShouldCreateStatusChangedList_WhenValidValues()
    {
        // Act
        var appointment = AppointmentFactory.CreateAppointment();

        // Assert
        appointment.Value!.StatusHistories.Should().NotBeNullOrEmpty();
    }
}
