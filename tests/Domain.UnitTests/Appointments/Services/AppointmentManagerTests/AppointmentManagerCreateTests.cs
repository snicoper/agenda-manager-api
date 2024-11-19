using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Services.Errors;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.Services.AppointmentManagerTests;

public class AppointmentManagerCreateTests : AppointmentManagerTestsBase
{
    [Fact]
    public async Task Create_ShouldSuccess_WhenValidValuesAreProvided()
    {
        // Arrange
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupOverlapPolicyIsOverlapping(Result.Success());
        SetupResourceAvailabilityPolicyIsAvailableAsync(Result.Success());
        SetupServiceRequirementsPolicyIsSatisfiedAsync(Result.Success());

        // Act
        var result = await CreateAppointmentManagerFactory();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenHolidayAvailabilityPolicyIsNotAvailable()
    {
        // Arrange
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Failure());

        // Act
        var result = await CreateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarHolidayErrors.HolidaysOverlap.FirstError());
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenOverlapPolicyIsOverlappingIsTrue()
    {
        // Arrange
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupOverlapPolicyIsOverlapping(Result.Failure(CalendarConfigurationErrors.KeyNotFound));

        // Act
        var result = await CreateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarConfigurationErrors.KeyNotFound.FirstError());
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenResourceAvailabilityPolicyIsNotAvailable()
    {
        // Arrange
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupOverlapPolicyIsOverlapping(Result.Success());
        SetupResourceAvailabilityPolicyIsAvailableAsync(Result.Failure(ResourceErrors.ResourceNotAvailable));

        // Act
        var result = await CreateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ResourceErrors.ResourceNotAvailable.FirstError());
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenServiceRequirementsPolicyIsNotSatisfied()
    {
        // Arrange
        SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync();
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupOverlapPolicyIsOverlapping(Result.Success());
        SetupResourceAvailabilityPolicyIsAvailableAsync(Result.Success());
        SetupServiceRequirementsPolicyIsSatisfiedAsync(Result.Failure(ServiceErrors.ServiceNotFound));

        // Act
        var result = await CreateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.ServiceNotFound.FirstError());
    }
}
