using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.AppointmentAggregate;

public class AppointmentChangeStateTests
{
    [Fact]
    public void ChangeState_ShouldSuccess_WithValidValues()
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointment(status: AppointmentStatus.Accepted);

        // Act
        appointment.Value!.ChangeState(AppointmentStatus.Waiting, "Waiting");

        // Assert
        appointment.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ChangeState_ShouldFailure_WithInvalidValues()
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointment(status: AppointmentStatus.Accepted);

        // Act
        var result = appointment.Value!.ChangeState(AppointmentStatus.InProgress, "InProgress");

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
