using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarHolidayManagerTests;

public class CalendarHolidayManagerTestsBase
{
    protected CalendarHolidayManagerTestsBase()
    {
        CalendarRepository = Substitute.For<ICalendarRepository>();
        AppointmentRepository = Substitute.For<IAppointmentRepository>();

        Sut = new CalendarHolidayManager(
            calendarRepository: CalendarRepository,
            appointmentRepository: AppointmentRepository);
    }

    protected ICalendarRepository CalendarRepository { get; }

    protected IAppointmentRepository AppointmentRepository { get; }

    protected CalendarHolidayManager Sut { get; }
}
