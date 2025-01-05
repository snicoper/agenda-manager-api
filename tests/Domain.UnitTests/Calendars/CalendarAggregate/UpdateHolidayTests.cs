using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class UpdateHolidayTests
{
    [Fact]
    public void UpdateHoliday_ShouldSuccess_WhenUpdateHoliday()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();
        var updatedHoliday = CalendarHolidayFactory.CreateCalendarHoliday(
            calendarHolidayId: holiday.Id,
            calendarId: holiday.CalendarId,
            period: Period.From(DateTimeOffset.UtcNow, DateTimeOffset.MaxValue),
            name: "New Value");
        calendar.AddHoliday(holiday);

        // Act
        calendar.UpdateHoliday(holiday, updatedHoliday.Period, updatedHoliday.Name);

        // Assert
        calendar.Holidays[0].Name.Should().Be("New Value");
    }

    [Fact]
    public void UpdateHoliday_ShouldRaiseEvent_WhenUpdateHoliday()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();
        var updatedHoliday = CalendarHolidayFactory.CreateCalendarHoliday(
            calendarHolidayId: holiday.Id,
            calendarId: holiday.CalendarId,
            period: Period.From(DateTimeOffset.UtcNow, DateTimeOffset.MaxValue),
            name: "New Value");
        calendar.AddHoliday(holiday);

        // Act
        calendar.UpdateHoliday(holiday, updatedHoliday.Period, updatedHoliday.Name);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarHolidayUpdatedDomainEvent);
    }

    [Fact]
    public void UpdateHoliday_ShouldNotRaiseEvent_WhenUpdateHolidayWithSameValue()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();
        calendar.AddHoliday(holiday);

        // Act
        calendar.UpdateHoliday(holiday, holiday.Period, holiday.Name);

        // Assert
        calendar.DomainEvents.Should().NotContain(x => x is CalendarHolidayUpdatedDomainEvent);
    }
}
