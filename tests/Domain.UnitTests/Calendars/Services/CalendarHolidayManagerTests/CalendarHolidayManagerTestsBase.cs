using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarHolidayManagerTests;

public class CalendarHolidayManagerTestsBase
{
    public CalendarHolidayManagerTestsBase()
    {
        CalendarRepository = Substitute.For<ICalendarRepository>();
        CalendarConfigurationRepository = Substitute.For<ICalendarConfigurationRepository>();
        AppointmentRepository = Substitute.For<IAppointmentRepository>();

        Sut = new CalendarHolidayManager(
            calendarRepository: CalendarRepository,
            calendarConfigurationRepository: CalendarConfigurationRepository,
            appointmentRepository: AppointmentRepository);
    }

    protected ICalendarRepository CalendarRepository { get; }

    protected ICalendarConfigurationRepository CalendarConfigurationRepository { get; }

    protected IAppointmentRepository AppointmentRepository { get; }

    protected CalendarHolidayManager Sut { get; }
}
