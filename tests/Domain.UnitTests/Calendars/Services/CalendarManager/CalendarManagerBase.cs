using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarManager;

public abstract class CalendarManagerBase
{
    protected CalendarManagerBase()
    {
        CalendarRepository = Substitute.For<ICalendarRepository>();
        AppointmentsInCalendarPolicy = Substitute.For<IHasAppointmentsInCalendarPolicy>();
        ResourcesInCalendarPolicy = Substitute.For<IHasResourcesInCalendarPolicy>();
        ServicesInCalendarPolicy = Substitute.For<IHasServicesInCalendarPolicy>();

        Sut = new CalendarManagerCreate(
            CalendarRepository,
            AppointmentsInCalendarPolicy,
            ResourcesInCalendarPolicy,
            ServicesInCalendarPolicy);
    }

    protected ICalendarRepository CalendarRepository { get; }

    protected IHasAppointmentsInCalendarPolicy AppointmentsInCalendarPolicy { get; }

    protected IHasResourcesInCalendarPolicy ResourcesInCalendarPolicy { get; }

    protected IHasServicesInCalendarPolicy ServicesInCalendarPolicy { get; }

    protected CalendarManagerCreate Sut { get; }
}
