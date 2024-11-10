using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exxceptions;
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
    public void Calendar_ShouldRaiseDomainEvent_WhenCalendarIsCreated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarCreatedDomainEvent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Calendar_ShouldThrowException_WhenCalendarIsCreatedWithInvalidName(int nameLength)
    {
        // Arrange
        var name = new string('a', nameLength);

        // Assert
        Assert.Throws<CalendarDomainException>(() => CalendarFactory.CreateCalendar(name: name));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Calendar_ShouldThrowException_WhenCalendarIsCreatedWithInvalidDescription(int descriptionLength)
    {
        // Arrange
        var description = new string('a', descriptionLength);

        // Assert
        Assert.Throws<CalendarDomainException>(() => CalendarFactory.CreateCalendar(description: description));
    }
}
