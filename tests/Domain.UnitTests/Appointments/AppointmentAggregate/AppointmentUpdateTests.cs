using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Resources;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.AppointmentAggregate;

public class AppointmentUpdateTests
{
    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Accepted)]
    public void Update_ShouldSuccess_WithValidValues(AppointmentStatus status)
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointment(status: status);
        var newPeriod = PeriodFactory.Create();
        List<Resource> newResources = [ResourceFactory.CreateResource(), ResourceFactory.CreateResource()];

        // Act
        appointment.Value!.Update(newPeriod, newResources);

        // Assert
        appointment.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public void Update_ShouldFailure_WhenStatusIsInvalid(AppointmentStatus status)
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointmentForTesting(status: status);
        var newPeriod = PeriodFactory.Create();

        // Act
        appointment.Value!.ChangeState(AppointmentStatus.Waiting);
        var result = appointment.Value!.Update(newPeriod, appointment.Value!.Resources.ToList());
        result.Error?.FirstError().Should().Be(AppointmentErrors.OnlyPendingAndAcceptedAllowed.FirstError());

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Update_ShouldFailure_WhenResourcesPassedAreEmpty()
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointment();
        var newPeriod = PeriodFactory.Create();

        // Act
        var result = appointment.Value!.Update(newPeriod, []);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(AppointmentErrors.NoResourcesProvided.FirstError());
    }

    [Fact]
    public void Update_ShouldNotRaiseEvent_WhenUpdateWithSameValues()
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointment();

        // Act
        var result = appointment.Value!.Update(appointment.Value!.Period, appointment.Value!.Resources.ToList());

        // Assert
        result.IsSuccess.Should().BeTrue();
        appointment.Value?.DomainEvents.Should().NotContain(x => x is AppointmentUpdatedDomainEvent);
    }
}
