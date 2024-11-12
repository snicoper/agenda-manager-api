using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarCreateTests
{
    [Fact]
    public void Calendar_ShouldReturnCalendar_WhenCalendarIsCreated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Assert
        calendar.Should().NotBeNull();
        calendar.Id.Should().Be(calendar.Id);
        calendar.Name.Should().Be(calendar.Name);
        calendar.Description.Should().Be(calendar.Description);
    }

    [Fact]
    public void Calendar_ShouldActiveTrue_WhenCalendarIsCreated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Assert
        calendar.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Calendar_ShouldRaiseDomainEvent_WhenCalendarIsCreated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarCreatedDomainEvent);
    }
}
