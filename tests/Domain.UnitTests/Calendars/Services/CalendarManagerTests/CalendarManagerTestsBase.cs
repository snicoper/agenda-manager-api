using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarManagerTests;

public abstract class CalendarManagerTestsBase
{
    protected CalendarManagerTestsBase()
    {
        CalendarRepository = Substitute.For<ICalendarRepository>();
        AppointmentsInCalendarPolicy = Substitute.For<IHasAppointmentsInCalendarPolicy>();
        ResourcesInCalendarPolicy = Substitute.For<IHasResourcesInCalendarPolicy>();
        ServicesInCalendarPolicy = Substitute.For<IHasServicesInCalendarPolicy>();

        Sut = new CalendarManager(
            CalendarRepository,
            AppointmentsInCalendarPolicy,
            ResourcesInCalendarPolicy,
            ServicesInCalendarPolicy);
    }

    protected ICalendarRepository CalendarRepository { get; }

    protected IHasAppointmentsInCalendarPolicy AppointmentsInCalendarPolicy { get; }

    protected IHasResourcesInCalendarPolicy ResourcesInCalendarPolicy { get; }

    protected IHasServicesInCalendarPolicy ServicesInCalendarPolicy { get; }

    protected CalendarManager Sut { get; }
}
