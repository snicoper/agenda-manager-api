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
        CalendarHolidayRepository = Substitute.For<ICalendarHolidayRepository>();
        CalendarHolidayAvailabilityPolicy = Substitute.For<ICalendarHolidayAvailabilityPolicy>();
        CalendarHolidayAvailabilityExcludeSelfPolicy = Substitute.For<ICalendarHolidayAvailabilityExcludeSelfPolicy>();
        AppointmentRepository = Substitute.For<IAppointmentRepository>();

        Sut = new CalendarHolidayManager(
            calendarRepository: CalendarRepository,
            calendarHolidayRepository: CalendarHolidayRepository,
            calendarHolidayAvailabilityPolicy: CalendarHolidayAvailabilityPolicy,
            calendarHolidayAvailabilityExcludeSelfPolicy: CalendarHolidayAvailabilityExcludeSelfPolicy,
            appointmentRepository: AppointmentRepository);
    }

    protected ICalendarRepository CalendarRepository { get; }

    protected ICalendarHolidayRepository CalendarHolidayRepository { get; }

    protected ICalendarHolidayAvailabilityPolicy CalendarHolidayAvailabilityPolicy { get; }

    protected ICalendarHolidayAvailabilityExcludeSelfPolicy CalendarHolidayAvailabilityExcludeSelfPolicy { get; }

    protected IAppointmentRepository AppointmentRepository { get; }

    protected CalendarHolidayManager Sut { get; }
}
