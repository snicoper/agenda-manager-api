using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.Services.AppointmentManagerTests;

public class AppointmentManagerDeleteTests : AppointmentManagerTestsBase
{
    [Fact]
    public async Task Delete_ShouldSuccess_WhenValidValuesAreProvided()
    {
        // Arrange
        SetupAppointmentRepositoryGetByIdAsync();

        // Act
        var result = await DeleteAppointmentManagerFactory();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldFailure_WhenAppointmentNotFound()
    {
        // Arrange
        SetupAppointmentRepositoryGetByIdAsync(createAppointment: false);

        // Act
        var result = await DeleteAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(AppointmentErrors.AppointmentNotFound.FirstError());
    }

    [Theory]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public async Task Delete_ShouldFailure_WhenStatusIsInvalid(AppointmentStatus status)
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointmentForTesting(status: status).Value;
        SetupAppointmentRepositoryGetByIdAsync(appointment);

        // Act
        var result = await DeleteAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(AppointmentErrors.AppointmentStatusInvalidForDelete.FirstError());
    }

    private Task<Result> DeleteAppointmentManagerFactory(AppointmentId? appointmentId = null)
    {
        var appointmentDeleted = Sut.DeleteAppointmentAsync(
            appointmentId: appointmentId ?? AppointmentId.Create(),
            cancellationToken: CancellationToken.None);

        return appointmentDeleted;
    }
}
