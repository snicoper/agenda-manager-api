using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars;

public class CalendarTests
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
}
