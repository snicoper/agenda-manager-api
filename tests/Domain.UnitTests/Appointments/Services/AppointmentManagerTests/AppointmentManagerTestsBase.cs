using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.Services;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.TestCommon.Factories;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Appointments.Services.AppointmentManagerTests;

public abstract class AppointmentManagerTestsBase
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICalendarHolidayAvailabilityPolicy _holidayAvailabilityPolicy;
    private readonly ICalendarWeekDayAvailabilityPolicy _weekDayAvailabilityPolicy;
    private readonly IAppointmentConfirmationStrategyPolicy _confirmationStrategyPolicy;
    private readonly IAppointmentOverlapPolicy _overlapPolicy;
    private readonly IResourceAvailabilityPolicy _resourceAvailabilityPolicy;
    private readonly IServiceRequirementsPolicy _serviceRequirementsPolicy;

    protected AppointmentManagerTestsBase()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _calendarRepository = Substitute.For<ICalendarRepository>();
        _holidayAvailabilityPolicy = Substitute.For<ICalendarHolidayAvailabilityPolicy>();
        _weekDayAvailabilityPolicy = Substitute.For<ICalendarWeekDayAvailabilityPolicy>();
        _confirmationStrategyPolicy = Substitute.For<IAppointmentConfirmationStrategyPolicy>();
        _overlapPolicy = Substitute.For<IAppointmentOverlapPolicy>();
        _resourceAvailabilityPolicy = Substitute.For<IResourceAvailabilityPolicy>();
        _serviceRequirementsPolicy = Substitute.For<IServiceRequirementsPolicy>();

        Sut = new AppointmentManager(
            _appointmentRepository,
            _calendarRepository,
            _holidayAvailabilityPolicy,
            _weekDayAvailabilityPolicy,
            _confirmationStrategyPolicy,
            _overlapPolicy,
            _resourceAvailabilityPolicy,
            _serviceRequirementsPolicy);
    }

    protected AppointmentManager Sut { get; }

    protected void SetupCreationStrategyPolicyDetermineInitialStatus(Result<AppointmentStatus>? result = null)
    {
        result ??= Result.Success(AppointmentStatus.Accepted);

        _confirmationStrategyPolicy.DetermineInitialStatus(Arg.Any<Calendar>())
            .Returns(result);
    }

    protected void SetupCalendarRepositoryGetByIdWithSettingsAsync(Calendar? calendarResult = null)
    {
        _calendarRepository
            .GetByIdWithSettingsAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(calendarResult);
    }

    protected void SetupOverlapPolicyIsOverlapping(Result? result = null)
    {
        result ??= Result.Success();

        _overlapPolicy
            .IsOverlappingAsync(
                Arg.Any<Calendar>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(result);
    }

    protected void SetupWeekDayAvailabilityPolicyIsAvailable(Result? result = null)
    {
        result ??= Result.Success();

        _weekDayAvailabilityPolicy
            .IsAvailable(Arg.Any<Calendar>(), Arg.Any<Period>())
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
                Arg.Any<Calendar>(),
                Arg.Any<List<Resource>>(),
                Arg.Any<Period>(),
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
