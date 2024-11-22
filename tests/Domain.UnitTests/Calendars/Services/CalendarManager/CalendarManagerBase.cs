using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarManager;

public abstract class CalendarManagerBase
{
    protected CalendarManagerBase()
    {
        CalendarRepository = Substitute.For<ICalendarRepository>();
        CalendarNameValidationPolicy = Substitute.For<ICalendarNameValidationPolicy>();
        AppointmentsInCalendarPolicy = Substitute.For<IHasAppointmentsInCalendarPolicy>();
        ResourcesInCalendarPolicy = Substitute.For<IHasResourcesInCalendarPolicy>();
        ServicesInCalendarPolicy = Substitute.For<IHasServicesInCalendarPolicy>();

        Sut = new Domain.Calendars.Services.CalendarManager(
            CalendarRepository,
            CalendarNameValidationPolicy,
            AppointmentsInCalendarPolicy,
            ResourcesInCalendarPolicy,
            ServicesInCalendarPolicy);
    }

    protected ICalendarRepository CalendarRepository { get; }

    protected ICalendarNameValidationPolicy CalendarNameValidationPolicy { get; }

    protected IHasAppointmentsInCalendarPolicy AppointmentsInCalendarPolicy { get; }

    protected IHasResourcesInCalendarPolicy ResourcesInCalendarPolicy { get; }

    protected IHasServicesInCalendarPolicy ServicesInCalendarPolicy { get; }

    protected Domain.Calendars.Services.CalendarManager Sut { get; }
}
