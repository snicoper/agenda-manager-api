using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.Services;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.TestCommon.Factories;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Appointments.Services.AppointmentManagerTests;

public abstract class AppointmentManagerTestsBase
{
    private readonly ICalendarConfigurationRepository _configurationRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICalendarHolidayAvailabilityPolicy _holidayAvailabilityPolicy;
    private readonly IAppointmentCreationStrategyPolicy _creationStrategyPolicy;
    private readonly IAppointmentOverlapPolicy _overlapPolicy;
    private readonly IResourceAvailabilityPolicy _resourceAvailabilityPolicy;
    private readonly IServiceRequirementsPolicy _serviceRequirementsPolicy;

    protected AppointmentManagerTestsBase()
    {
        _configurationRepository = Substitute.For<ICalendarConfigurationRepository>();
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _holidayAvailabilityPolicy = Substitute.For<ICalendarHolidayAvailabilityPolicy>();
        _creationStrategyPolicy = Substitute.For<IAppointmentCreationStrategyPolicy>();
        _overlapPolicy = Substitute.For<IAppointmentOverlapPolicy>();
        _resourceAvailabilityPolicy = Substitute.For<IResourceAvailabilityPolicy>();
        _serviceRequirementsPolicy = Substitute.For<IServiceRequirementsPolicy>();

        Sut = new AppointmentManager(
            _configurationRepository,
            _appointmentRepository,
            _holidayAvailabilityPolicy,
            _creationStrategyPolicy,
            _overlapPolicy,
            _resourceAvailabilityPolicy,
            _serviceRequirementsPolicy);
    }

    protected AppointmentManager Sut { get; }

    protected void SetupConfigurationRepositoryGetConfigurationsByCalendarIdAsync(
        List<CalendarConfiguration>? configurationsResult = null)
    {
        configurationsResult ??= [];

        _configurationRepository.GetConfigurationsByCalendarIdAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(configurationsResult);
    }

    protected void SetupCreationStrategyPolicyDetermineInitialStatus(Result<AppointmentStatus>? result = null)
    {
        result ??= Result.Success(AppointmentStatus.Accepted);

        _creationStrategyPolicy.DetermineInitialStatus(Arg.Any<List<CalendarConfiguration>>())
            .Returns(result);
    }

    protected void SetupOverlapPolicyIsOverlapping(Result? result = null)
    {
        result ??= Result.Success();

        _overlapPolicy
            .IsOverlappingAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<List<CalendarConfiguration>>(),
                Arg.Any<CancellationToken>())
            .Returns(result);
    }

    protected void SetupHolidayAvailabilityPolicyIsAvailable(Result? result = null)
    {
        result ??= Result.Success();

        _holidayAvailabilityPolicy
            .IsAvailableAsync(Arg.Any<CalendarId>(), Arg.Any<Period>(), Arg.Any<CancellationToken>())
            .Returns(result);
    }

    protected void SetupResourceAvailabilityPolicyIsAvailableAsync(Result? result = null)
    {
        result ??= Result.Success();

        _resourceAvailabilityPolicy
            .IsAvailableAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<List<Resource>>(),
                Arg.Any<Period>(),
                Arg.Any<List<CalendarConfiguration>>(),
                Arg.Any<CancellationToken>())
            .Returns(result);
    }

    protected void SetupServiceRequirementsPolicyIsSatisfiedAsync(Result? result = null)
    {
        result ??= Result.Success();

        _serviceRequirementsPolicy
            .IsSatisfiedByAsync(
                Arg.Any<ServiceId>(),
                Arg.Any<List<Resource>>(),
                Arg.Any<CancellationToken>())
            .Returns(result);
    }

    protected Appointment? SetupAppointmentRepositoryGetByIdAsync(
        Appointment? appointmentResult = null,
        bool createAppointment = true)
    {
        appointmentResult = appointmentResult is null && createAppointment
            ? AppointmentFactory.CreateAppointment().Value
            : appointmentResult;

        _appointmentRepository
            .GetByIdAsync(Arg.Any<AppointmentId>(), Arg.Any<CancellationToken>())
            .Returns(appointmentResult);

        return appointmentResult;
    }
}
