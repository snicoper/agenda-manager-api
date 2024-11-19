using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.Services.AppointmentManagerTests;

public class AppointmentManagerUpdateTests : AppointmentManagerTestsBase
{
    [Fact]
    public async Task Update_ShouldSuccess_WhenValidValuesAreProvided()
    {
        // Arrange
        SetupAppointmentRepositoryGetByIdAsync();
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupOverlapPolicyIsOverlapping(Result.Success());
        SetupResourceAvailabilityPolicyIsAvailableAsync(Result.Success());
        SetupServiceRequirementsPolicyIsSatisfiedAsync(Result.Success());

        // Act
        var result = await UpdateAppointmentManagerFactory();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenAppointmentNotFound()
    {
        // Arrange
        SetupAppointmentRepositoryGetByIdAsync(createAppointment: false);

        // Act
        var result = await UpdateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(AppointmentErrors.AppointmentNotFound.FirstError());
    }

    [Theory]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public async Task Update_ShouldFailure_WhenCurrentValueAreNotValid(AppointmentStatus status)
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointmentForTesting(status: status).Value;

        SetupAppointmentRepositoryGetByIdAsync(appointment);
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();

        // Act
        var result = await UpdateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(AppointmentErrors.AppointmentStatusInvalidForUpdate.FirstError());
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenHolidayAvailabilityPolicyIsNotAvailable()
    {
        // Arrange
        SetupAppointmentRepositoryGetByIdAsync();
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Failure());

        // Act
        var result = await UpdateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarHolidayErrors.HolidaysOverlap.FirstError());
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenOverlapPolicyIsOverlappingIsTrue()
    {
        // Arrange
        SetupAppointmentRepositoryGetByIdAsync();
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupOverlapPolicyIsOverlapping(Result.Failure());

        // Act
        var result = await UpdateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(AppointmentErrors.AppointmentsOverlapping.FirstError());
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenResourceAvailabilityPolicyIsNotAvailable()
    {
        // Arrange
        SetupAppointmentRepositoryGetByIdAsync();
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupOverlapPolicyIsOverlapping(Result.Failure());
        SetupResourceAvailabilityPolicyIsAvailableAsync(Result.Failure());

        // Act
        var result = await UpdateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ResourceErrors.ResourceNotAvailable.FirstError());
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenServiceRequirementsPolicyIsNotSatisfied()
    {
        // Arrange
        SetupAppointmentRepositoryGetByIdAsync();
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupOverlapPolicyIsOverlapping(Result.Failure());
        SetupResourceAvailabilityPolicyIsAvailableAsync(Result.Success());
        SetupServiceRequirementsPolicyIsSatisfiedAsync(Result.Failure());

        // Act
        var result = await UpdateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.ResourceRequirementsMismatch.FirstError());
    }

    private Task<Result<Appointment>> UpdateAppointmentManagerFactory(
        AppointmentId? appointmentId = null,
        Period? period = null,
        List<Resource>? resources = null)
    {
        resources ??=
        [
            ResourceFactory.CreateResource(),
            ResourceFactory.CreateResource()
        ];

        var appointmentUpdated = Sut.UpdateAppointmentAsync(
            appointmentId: appointmentId ?? AppointmentId.Create(),
            period: period ?? PeriodFactory.Create(),
            resources: resources ?? [],
            cancellationToken: CancellationToken.None);

        return appointmentUpdated;
    }
}
