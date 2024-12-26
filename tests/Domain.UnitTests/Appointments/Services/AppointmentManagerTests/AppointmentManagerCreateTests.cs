using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using AgendaManager.TestCommon.Factories.ValueObjects;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.Services.AppointmentManagerTests;

public class AppointmentManagerCreateTests : AppointmentManagerTestsBase
{
    [Fact]
    public async Task Create_ShouldSuccess_WhenValidValuesAreProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);
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
    public async Task Create_ShouldFailure_WhenCalendarNotFound()
    {
        // Arrange
        SetupCalendarRepositoryGetByIdWithSettingsAsync();

        // Act
        var result = await CreateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarErrors.CalendarNotFound.FirstError());
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenHolidayAvailabilityPolicyIsNotAvailable()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);
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
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupHolidayAvailabilityPolicyIsAvailable(Result.Success());
        SetupCreationStrategyPolicyDetermineInitialStatus();
        SetupOverlapPolicyIsOverlapping(Result.Failure(AppointmentErrors.AppointmentsOverlapping));

        // Act
        var result = await CreateAppointmentManagerFactory();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(AppointmentErrors.AppointmentsOverlapping.FirstError());
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenResourceAvailabilityPolicyIsNotAvailable()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);
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
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarRepositoryGetByIdWithSettingsAsync(calendar);
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

    private async Task<Result<Appointment>> CreateAppointmentManagerFactory(
        CalendarId? calendarId = null,
        ServiceId? serviceId = null,
        UserId? userId = null,
        Period? period = null,
        List<Resource>? resources = null)
    {
        resources ??=
        [
            ResourceFactory.CreateResource(),
            ResourceFactory.CreateResource()
        ];

        var appointmentCreated = await Sut.CreateAppointmentAsync(
            calendarId: calendarId ?? CalendarId.Create(),
            serviceId: serviceId ?? ServiceId.Create(),
            userId: userId ?? UserId.Create(),
            period: period ?? PeriodFactory.Create(),
            resources: resources,
            cancellationToken: CancellationToken.None);

        return appointmentCreated;
    }
}
